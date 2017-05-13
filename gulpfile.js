require('dotenv').config()

var path = require('path');
var gulp = require('gulp');
var del = require('del');


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
gulp.task('build', ['restore'], ()=>{
    return gulp.src('**/*.csproj', {read: false})
        .pipe(build({configuration: configuration, version: version}));
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
  return del([
    'nuget_packages/*'
  ]);
});

gulp.task('deploy_solution',['clean:nuget', 'push']);