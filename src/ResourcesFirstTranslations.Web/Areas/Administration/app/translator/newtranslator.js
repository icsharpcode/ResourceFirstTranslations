(function () {
    'use strict';
    var controllerId = 'newtranslator';
    angular.module('app').controller(controllerId, ['$scope','common', 'datacontext', '$location','bootstrap.dialog', newtranslator]);

    function newtranslator($scope, common, datacontext, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'New translator';
        vm.emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        vm.culturePattern = /^[|].+[|]$/;
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
                return datacontext.saveNewTranslator(vm.item).done(function (result) {
                    var scope = angular.element($("#newtranslator-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
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

        vm.clearValues = function() {
            return vm.item = {
                Id: 0,
                FirstName: "",
                LastName: "",
                UserName: "",
                EmailAddress: "",
                Cultures: "",
                IsActive: false,
                IsAdmin: false
            }, vm.submitted = false;
            
        }

        

        function activate() {
            var promises = [getTranslator()];
            common.activateController(promises, controllerId)
                .then(function () {});
        }
        function getTranslator() {
            return vm.item = {
                Id: 0,
                FirstName: "",
                LastName: "",
                UserName: "",
                EmailAddress: "",
                Cultures: "",
                IsActive: false,
                IsAdmin: false
            };

        }
        
        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }
        activate();
    }
})();