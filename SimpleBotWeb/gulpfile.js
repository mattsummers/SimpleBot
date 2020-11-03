/// <binding BeforeBuild='min' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. https://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp'),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

gutil = require('gulp-util');

var paths = {
    webroot: "./"
};

// Uglify doesn't work on ES6 scripts currently
paths.js = [
    paths.webroot + "wwwroot/lib/jquery-3.2.1/jquery-3.2.1.js",
    paths.webroot + "wwwroot/lib/popper-1.12.9/popper.js",
    paths.webroot + "wwwroot/lib/bootstrap-4.0.0-beta.2/js/bootstrap.js",
    paths.webroot + "wwwroot/lib/bootbox-4.4.0/bootbox.min.js",
    paths.webroot + "wwwroot/js/site.js"
];

//paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = [
    paths.webroot + "wwwroot/lib/bootstrap-4.0.0-beta.2/css/bootstrap.css",
    paths.webroot + "wwwroot/css/site.css"
];

//paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "wwwroot/js/site.min.js";
paths.concatCssDest = paths.webroot + "wwwroot/css/site.min.css";

gulp.task("clean:js", function (cb) {
    return rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    return rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", gulp.parallel("clean:js", "clean:css"));

gulp.task("min:js", function () {
    return gulp.src(paths.js, { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .on('error', function (err) { gutil.log(gutil.colors.red('[Error]'), err.toString()); })
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src(paths.css)
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", gulp.parallel("min:js", "min:css"));
gulp.task("default", gulp.parallel("min:js", "min:css"))