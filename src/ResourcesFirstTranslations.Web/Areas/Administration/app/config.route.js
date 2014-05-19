(function () {
    'use strict';

    var app = angular.module('app');

    // Collect the routes
    app.constant('routes', getRoutes());

    // Configure the routes and route resolvers
    app.config(['$routeProvider', 'routes', routeConfigurator]);
    function routeConfigurator($routeProvider, routes) {

        routes.forEach(function (r) {
            $routeProvider.when(r.url, r.config);
        });
        $routeProvider.otherwise({ redirectTo: '/' });
    }

    function getRoutes() {
        return [
            {
                url: '/',
                config: {
                    templateUrl: '/Areas/Administration/app/dashboard/dashboard.html',
                    title: 'dashboard',
                    settings: {
                        nav: 1,
                        content: '<i class="fa fa-dashboard"></i> Dashboard'
                    }
                }
            },
            {
                url: '/translator',
                config: {
                    title: 'Translators',
                    templateUrl: '/Areas/Administration/app/translator/translators.html',
                    settings: {
                        nav: 2,
                        content: '<i class="fa fa-users"></i> Translators'
                    }
                }
            }, {
                url: '/translator/:id',
                config: {
                    title: 'Translators',
                    templateUrl: '/Areas/Administration/app/translator/edittranslator.html',
                    settings: {

                    }
                }
            }, {
                url: '/newtranslator',
                config: {
                    title: 'Translators',
                    templateUrl: '/Areas/Administration/app/translator/newtranslator.html',
                    settings: {

                    }
                }
            }, {
                url: '/resetpassword/:id',
                config: {
                    title: 'Translators',
                    templateUrl: '/Areas/Administration/app/translator/resetpassword.html',
                    settings: {

                    }
                }
            }, {
                url: '/branches',
                config: {
                    title: 'branches',
                    templateUrl: '/Areas/Administration/app/branches/branches.html',
                    settings: {
                        nav: 3,
                        content: '<i class="fa fa-asterisk"></i> Branches'
                    }
                }
            }, {
                url: '/branches/:id',
                config: {
                    title: 'branches',
                    templateUrl: '/Areas/Administration/app/branches/editbranch.html',
                    settings: {

                    }
                }
            }, {
                url: '/newbranch',
                config: {
                    title: 'branches',
                    templateUrl: '/Areas/Administration/app/branches/newbranch.html',
                    settings: {

                    }
                }
            }, {
                url: '/branchresourcefiles',
                config: {
                    title: 'branchresourcefiles',
                    templateUrl: '/Areas/Administration/app/branchesresourcefiles/branchesfiles.html',
                    settings: {
                        nav: 5,
                        content: '<i class="fa fa-asterisk"></i> Branch Files'
                    }
                }
            }, {
                url: '/branchesresourcefiles/:id',
                config: {
                    title: 'branchresourcefiles',
                    templateUrl: '/Areas/Administration/app/branchesresourcefiles/editbranchfile.html',
                    settings: {

                    }
                }
            }, {
                url: '/newbranchfile',
                config: {
                    title: 'branchresourcefiles',
                    templateUrl: '/Areas/Administration/app/branchesresourcefiles/newbranchfile.html',
                    settings: {

                    }
                }
            }, {
                url: '/languages',
                config: {
                    title: 'languages',
                    templateUrl: '/Areas/Administration/app/languages/languages.html',
                    settings: {
                        nav: 6,
                        content: '<i class="fa fa-asterisk"></i> Languages'
                    }
                }
            }, {
                url: '/newlanguage',
                config: {
                    title: 'languages',
                    templateUrl: '/Areas/Administration/app/languages/newlanguage.html',
                    settings: {

                    }
                }
            }, {
                url: '/resourcefiles',
                config: {
                    title: 'resourcefiles',
                    templateUrl: '/Areas/Administration/app/resourcefiles/resourcefiles.html',
                    settings: {
                        nav: 4,
                        content: '<i class="fa fa-asterisk"></i> Resource Files'
                    }
                }
            }, {
                url: '/resourcefiles/:id',
                config: {
                    title: 'resourcefiles',
                    templateUrl: '/Areas/Administration/app/resourcefiles/editresourcefile.html',
                    settings: {

                    }
                }
            }, {
                url: '/newresourcefile',
                config: {
                    title: 'resourcefiles',
                    templateUrl: '/Areas/Administration/app/resourcefiles/newresourcefile.html',
                    settings: {

                    }
                }
            }, {
                url: '/sendmail',
                config: {
                    title: 'sendmail',
                    templateUrl: '/Areas/Administration/app/sendmail/sendmail.html',
                    settings: {
                        nav: 7,
                        content: '<i class="fa fa-mail-forward"></i> Send Mail'
                    }
                }
            }
        ];
    }
})();