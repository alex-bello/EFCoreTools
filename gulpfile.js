require('dotenv').config()

var path = require('path');
var gulp = require('gulp');
var debug = require('gulp-debug');
var del = require('del');
var gutil = require('gulp-util');
var increment = require('version-incrementer').increment;
var jeditor = require('gulp-json-editor');
var runSequence = require('run-sequence');
var wait = require('gulp-wait');
let {restore, build, test, pack, push} = require('gulp-dotnet-cli-v1');

/* ENVIRONMENT VARIABLES */
var configuration = process.env.BUILD_CONFIGURATION || 'Debug';
var fileList = ['src/**/project.json', '!**/bin/**/*', '!tests/**'];

gulp.task('clean:nuget', ['wait'], () => {
  return del(['nuget_packages/*']);
});

gulp.task('clean:bin_folders', ['wait'], () => {
    return del(['src/**/bin/']);
});

gulp.task('increment', () => {
    return gulp.src(['src/**/project.json', 'package.json', '!**/bin/**/*'], {base: "./"})
        .pipe(debug())
        .pipe(jeditor(function(json) {
            var versionSplit = json.version.split('-alpha');
            json.version = (configuration == 'Debug') ? versionSplit[0] + '-alpha' + ((versionSplit[1]) ? (parseInt(versionSplit[1]) + 1) : 1) : increment(versionSplit);
            return json; // must return JSON object. 
        }))
        .pipe(gulp.dest("./"));
});

// restore nuget packages 
gulp.task('restore', ()=>{
    return gulp.src(fileList, {read: false})
            .pipe(restore());
})

// compile all projects in solution file(s)
gulp.task('build', ['restore'], ()=>{
    return gulp.src(fileList, {read: false})
        .pipe(build({
            configuration: configuration
        }));
});

// convert a project to a nuget package 
gulp.task('pack', ()=>{
    return gulp.src(fileList, {read: false})
        .pipe(pack({
            noBuild: true,
            output: path.join(process.cwd(), 'nuget_packages'), 
            configuration: configuration
        }));
});

//push nuget packages to a server 
gulp.task('push', () => {
    return gulp.src(['nuget_packages/*.nupkg'], {read: false})
        .pipe(push({
            apiKey: process.env.NUGET_API_KEY, 
            source: process.env.NUGET_SOURCE
        }));
});

// run all unit test projects
gulp.task('test', ()=>{
    return gulp.src('tests/**/project.json', {read: false})
        .pipe(test())
});

gulp.task('wait', () => {
    gutil.log('Waiting 10 seconds...'); 
    wait(10000); // Wait 10 seconds for all file locks to be removed.
    gutil.log('Wait completed, continuing...');
});

//gulp.task('deploy',['increment', 'clean:nuget', 'push']);
gulp.task('build_sequence', () => {
	runSequence(
        'clean:bin_folders',
        'wait',
        'build',
        'wait',
        'pack'
	);
});

//gulp.task('deploy',['increment', 'clean:nuget', 'push']);
gulp.task('deploy', () => {
	runSequence(
        'increment',
        'clean:nuget',
        'clean:bin_folders',
        'wait',
        'build',
        'wait',
        'pack',
        'wait',
        'push'
	);
});