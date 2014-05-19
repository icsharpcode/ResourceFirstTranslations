(function () {
    'use strict';
    var controllerId = 'edittranslator';
    angular.module('app').controller(controllerId, ['$scope', '$rootScope', 'common', 'datacontext', '$routeParams', '$location', 'bootstrap.dialog', edittranslator]);

    function edittranslator($scope, $rootScope, common, datacontext, $routeParams, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'Edit translator';
        vm.emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        vm.culturePattern = /^[|].+[|]$/;
        var onRouteChangeOff;
        vm.currentTranslator = $routeParams.id;
        vm.busyMessage = 'Please wait ...';
        vm.isBusy = false;
        vm.spinnerOptions = {
            radius: 40,
            lines: 7,
            length: 0,
            width: 30,
            speed: 1.7,
            corners: 1.0,
            trail: 100,
            color: '#F58A00'
        };
        vm.goBack = function () {
            $location.path("/translator");
        }
        vm.Save = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                return datacontext.editTranslator(vm.editItem).done(function(result) {
                    var scope = angular.element($("#edittranslator-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                            onRouteChangeOff(); //Stop listening for location changes
                            $location.path('/translator');
                        });
                    } else {
                        vm.resultDialog("Error", result.Message);
                    }
                }).always(function() {
                    toggleSpinner(false);

                });
            }
            return false;
        }

        vm.isItemUnchanged = function () {
            if (!angular.equals(vm.editItem.Name, vm.item.Name)) {
                return false;
            }
            if (!angular.equals(vm.editItem.UserName, vm.item.UserName)) {
                return false;
            }
            if (!angular.equals(vm.editItem.FirstName, vm.item.FirstName)) {
                return false;
            }
            if (!angular.equals(vm.editItem.LastName, vm.item.LastName)) {
                return false;
            }
            if (!angular.equals(vm.editItem.EmailAddress, vm.item.EmailAddress)) {
                return false;
            }
            if (!angular.equals(vm.editItem.Cultures, vm.item.Cultures)) {
                return false;
            }
            if (!angular.equals(vm.editItem.IsActive, vm.item.IsActive)) {
                return false;
            }
            if (!angular.equals(vm.editItem.IsAdmin, vm.item.IsAdmin)) {
                return false;
            }
            return true;

        };

        vm.clearValues = function () {
            return vm.editItem = $.extend({}, vm.item), vm.submitted = false;
        };

        function activate() {
            var promises = [getTranslator()];
            common.activateController(promises, controllerId)
                .then(function() {
                    onRouteChangeOff = $rootScope.$on('$locationChangeStart', routeChange);
            });
        }

        function getTranslator() {
            return datacontext.getTranslator(vm.currentTranslator).then(function (data) {
                data.results.forEach(function (item) {
                    item.Name = item.FirstName + " " + item.LastName;
                });
                return vm.item = data.results[0], vm.editItem = $.extend({}, vm.item);
            }).fail(queryFailed);
        }

        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }

        function queryFailed(error) {
            logError(error.message, "Query failed", true);
        }

        function routeChange(event, next, current) {
            if (vm.isItemUnchanged()) return;
            dialog.confirmationDialog("RFT", "You have unsaved changes, do you want to continue?", "OK", "Cancel").then(function () {
                onRouteChangeOff(); //Stop listening for location changes
                $location.url($location.url(next).hash()); //Go to page they're interested in
            }).catch(function (e) {
            });
            //prevent navigation by default since we'll handle it
            //once the user selects a dialog option
            event.preventDefault();
            return;
        }

        activate();
    }

})();