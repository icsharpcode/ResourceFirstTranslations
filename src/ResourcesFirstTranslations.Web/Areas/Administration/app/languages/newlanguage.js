(function () {
    'use strict';
    var controllerId = 'newlanguage';
    angular.module('app').controller(controllerId, ['$scope', 'common', 'datacontext', '$location', 'bootstrap.dialog', newlanguage]);

    function newlanguage($scope, common, datacontext, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'New language';
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
            $location.path("/languages");
        }
        vm.Save = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                return datacontext.saveNewLanguage(vm.item).done(function (result) {
                    var scope = angular.element($("#newlanguage-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                            $location.path('/languages');
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

        vm.clearValues = function() {
            return vm.item = {
                Culture: "",
                Description: ""
            }, vm.submitted = false;
        }

        activate();

        function activate() {
            var promises = [getLanguage()];
            common.activateController(promises, controllerId)
                .then(function () { });
        }
        function getLanguage() {
            return vm.item = {
                Culture: "",
                Description: ""
            };

        }
        
        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }
    }
})();