;(function (global, factory) {
   typeof exports === 'object' && typeof module !== 'undefined'
       && typeof require === 'function' ? factory(require('~/themes/moment')) :
   typeof define === 'function' && define.amd ? define(['~/themes/moment'], factory) :
   factory(global.moment)
}(this, (function (moment) { 'use strict';
