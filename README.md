Resource-First Translations
=========================

RFT is the next generation Translation User eXperience built upon the insights
of the SharpDevelop project. It has the following goals:

* Code-First: Developers live in the IDE and work directly 
    with resource files. Source control is the source of truth for resources.
* Multi-Branch: Translations come and go with features being added, modified or removed.
    Translators should never duplicate work.
* Multi-File: Complex software doesn't ship with a single resource file only. Slice
    your application into modules and still get the benefits of translating only once.
* Sync: Automatically get the latest resource file checkins to the translators, and
    the finished translations to the developers or the build servers.

__Live demo__: http://rftdemo.azurewebsites.net/ 

Translator credentials: _translator_, _translator_  
Administrator credentials: _admin_, _administrator_    	

Certain operations are disallowed in the demo (eg creating new users, resetting a password)

__Videos / Screencasts__

[Administration Area](https://www.youtube.com/watch?v=_sXc_iwqy5s)
[Translators demo](https://www.youtube.com/watch?v=t7fSID0fFlw)

# Documentation

Please see the [wiki](https://github.com/icsharpcode/ResourceFirstTranslations/wiki)

# License

[MIT](https://github.com/icsharpcode/ResourceFirstTranslations/blob/master/LICENSE)

Written by [Andreas Lillich](https://github.com/andreaslillich) (frontend) 
and [Christoph Wille](https://github.com/christophwille) (backend). The specification
was co-written by Daniel Grunwald, Siegfried Pammer and Christoph Wille.

# Changelog

## July 28th, 2014

* RFT is in production use for SharpDevelop

## May 19th, go-live on Github

* A first non-production test version of RFT

## April 6th, 2014

Andreas joins in to create the SPA frontend applications

## Feb 25th, 2014

Start coding as a proof of concept to see if this can be pulled off

## SharpDevelop Developer Days 2013

Daniel, Siegfried and Christoph sat down and created a specification

## Years ago...

We had been talking about replacing the existing application for ages.