require('dotenv').config()

var path = require('path');
var gulp = require('gulp');
var debug = require('gulp-debug');
var del = require('del');
var gutil = require('gulp-util');
var runSequence = require('run-sequence');
var prettyjson = require('format-json-pretty');
var fs = require('fs');
let {restore, build, test, pack, push} = require('gulp-dotnet-cli');

/* GET package.json VALUES */
var packageJson = JSON.parse(fs.readFileSync('./package.json'));

/* SET VARIABLE VALUES */
var build_configuration = process.env.BUILD_CONFIGURATION || 'Debug';
var isDebug = (build_configuration.toLowerCase() === 'debug');

// Removes all files from the nuget_packages folder
gulp.task('clean:nuget', (cb) => {
  return del(['nuget_packages/*']);
  cb(err);
});

// Removes all files from all bin folders in any subdirectory
gulp.task('clean:bin_folders', (cb) => {
    return del(['src/**/bin/']);
    cb(err);
});

gulp.task('increment', (cb) => {
    packageJson.version = semverIncrementer(packageJson.version, isDebug);
    fs.writeFile('./package.json', prettyjson(packageJson, 4), cb);
    return;
});

// restore nuget packages 
gulp.task('restore', (cb) => {
    return gulp.src('*.sln', {read: false})
        .pipe(restore());
    cb(err);
})

// compile all projects in solution file(s)
gulp.task('build', (cb) => {
    return gulp.src('*.sln', {read: false})
        .pipe(build({
            configuration: build_configuration,
            echo: true,
            version: packageJson.version
        }));
    cb(err);
});

// convert a project to a nuget package 
gulp.task('pack', (cb) => {
    return gulp.src('*.sln', {read: false})
        .pipe(pack({
            configuration: build_configuration,
            includeSymbols: false,
            noBuild: true,
            output: path.join(process.cwd(), 'nuget_packages'),
            version: packageJson.version            
        }));
    cb(err);
});

//push nuget packages to a server 
gulp.task('push', () => {
    return gulp.src(['nuget_packages/*.nupkg', '!nuget_packages/*symbols.nupkg'], {read: false})
        .pipe(push({
            apiKey: process.env.NUGET_API_KEY, 
            source: process.env.NUGET_SOURCE
        }));
});

// run all unit test projects
gulp.task('test', (cb) => {
    return gulp.src('tests/**/*.csproj', {read: false})
        .pipe(test(
            {
                echo: true
            }
        ));
    cb(err);
});

gulp.task('wait', () => {
    
    gutil.log('Waiting 5 seconds...'); 
    wait(5000); // Wait 10 seconds for all file locks to be removed.
    gutil.log('Wait completed, continuing...');
    return;
});

gulp.task('try_build', (cb) => {
	return runSequence(
        'clean:nuget',
        'clean:bin_folders',
        'wait',
        'restore',
        'wait',
        'build',
        'wait',
        'test'
    );
    cb(err);
});

//gulp.task('deploy',['increment', 'clean:nuget', 'push']);
gulp.task('deploy', () => {
	runSequence(
        'clean:nuget',
        'clean:bin_folders',
        'wait',
        'restore',
        'wait',
        'build',
        'wait',
        'test',
        'wait',
        'increment',
        'wait',
        'pack',
        'wait',
        'push'
	);
});


/* CUSTOM FUNCTIONS */

function semverIncrementer(version, isPreRelease, preReleaseLabel = 'alpha') {
    
    // throw error if version does not satisfy semver spec
    if (!/^([0-9]{1,}\.){2,2}[0-9]{1,}([-][a-z]{1,}[0-9]{1,})?$/.test(version)) throw new Error ('version does not match semver format.')

    // split off preRelease
    var x = version.split('-');

    // if not preRelease, just increment the patch number and return
    if (!isPreRelease)
    {
        version = x[0].split('.');
        version[2] = parseInt(version[2]) + 1;
        return version.join('.');
    }

    // if preReleaseLabel matches current label, increment, else restart with new preReleaseLabel and return
    return x[0] + '-' + (x.length != 2 || !(x[1].startsWith(preReleaseLabel)) ? (preReleaseLabel + '1') : (preReleaseLabel + (parseInt(x[1].replace(preReleaseLabel, '')) + 1)));
};

function wait(ms){
    var start = new Date().getTime();
    var end = start;
    while(end < start + ms) {
      end = new Date().getTime();
   }
 }