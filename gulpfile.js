require('dotenv').config()

var path = require('path');
var gulp = require('gulp');
var del = require('del');
var increment = require('version-incrementer').increment;
var upsertEnv = require('upsert-env');


let {restore, build, test, pack, push} = require('gulp-dotnet-cli');
var d = new Date();
var buildNumber =
    d.getUTCFullYear() +
    ("0" + (d.getUTCMonth()+1)).slice(-2) +
    ("0" + d.getUTCDate()).slice(-2) +
    ("0" + d.getUTCHours()).slice(-2) +
    ("0" + d.getUTCMinutes()).slice(-2);

var version = process.env.VERSION_NUMBER || '1.0.0'; // Set version number for project
if (process.env.ALLOW_JULIAN_DATE_BUILD_NUMBER == "true") { version += '-' + buildNumber; } // If ALLOW_JULIAN_DATE_BUILD_NUMBER true, append build number
var configuration = process.env.BUILD_CONFIGURATION || 'Debug';


//restore nuget packages 
gulp.task('restore', ()=>{
    return gulp.src('**/*.csproj', {read: false})
            .pipe(restore());
})

//compile 
gulp.task('build', ['clean:bin_folders', 'restore'], ()=>{
    return gulp.src('**/*.csproj', {read: false})
        .pipe(build({configuration: configuration, version: version, echo: true}));
});

//run unit tests 
gulp.task('test', ['build'], ()=>{
    return gulp.src('**/*test*.csproj', {read: false})
        .pipe(test())
});

//convert a project to a nuget package 
gulp.task('pack', ['build'], ()=>{
    return gulp.src('**/*.csproj', {read: false})
        .pipe(pack({
            noBuild: true,
            output: path.join(process.cwd(), 'nuget_packages'), 
            version: version,
            configuration: configuration,
            includeSymbols: true
        }));
});

//push nuget packages to a server 
gulp.task('push', ['pack'], ()=>{
    return gulp.src('nuget_packages/*.nupkg', {read: false})
                .pipe(push({
                    apiKey: process.env.NUGET_API_KEY, 
                    source: process.env.NUGET_SOURCE}));
});

gulp.task('clean:nuget', function () {
  return del(['nuget_packages/*']);
});

gulp.task('clean:bin_folders', () => {
    return del(['src/**/bin/debug/']);
});

// Increments the build number by one. Used when deploying solution.
gulp.task('increment_build_number', () => {
    version = increment(version);
    upsertEnv.set({'VERSION_NUMBER': version});
    return;
    }
);

gulp.task('deploy',['increment_build_number', 'clean:nuget', 'push']);