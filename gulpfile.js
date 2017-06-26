require('dotenv').config()

var path = require('path');
var gulp = require('gulp');
var del = require('del');
var increment = require('version-incrementer').increment;
var jeditor = require('gulp-json-editor');
let {restore, build, test, pack, push} = require('gulp-dotnet-cli');

/* ENVIRONMENT VARIABLES */
var configuration = process.env.BUILD_CONFIGURATION || 'Debug';

// restore nuget packages 
gulp.task('restore', ()=>{
    return gulp.src('**/*.sln', {read: false})
            .pipe(restore());
})

// compile all projects in solution file(s)
gulp.task('build', ['clean:bin_folders', 'restore'], ()=>{
    return gulp.src('**/*.sln', {read: false})
        .pipe(build({configuration: configuration, version: version, echo: true}));
});

// run all unit test projects
gulp.task('test', ['build'], ()=>{
    return gulp.src('**/*test*.csproj', {read: false})
        .pipe(test())
});

// convert a project to a nuget package 
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
    return del(['src/**/bin/']);
});

gulp.task('increment', () => {
    return gulp.src('package.json')
        .pipe(jeditor(function(json) {
            var versionSplit = json.version.split('-alpha');
            json.version = (configuration == 'Debug') ? versionSplit[0] + '-alpha' + ((versionSplit[1]) ? (parseInt(versionSplit[1]) + 1) : 1) : increment(versionSplit);
          return json; // must return JSON object. 
        }))
        .pipe(gulp.dest("."));
});

gulp.task('deploy',['increment', 'clean:nuget', 'push']);