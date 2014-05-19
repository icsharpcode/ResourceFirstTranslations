(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['common', '$rootScope', '$scope', 'datacontext', 'bootstrap.dialog', dashboard]);

    function dashboard(common, $rootScope, $scope, datacontext, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.Translator = {
            Name: "",
            Languages: [],
            Email: ""
        }
        vm.title = 'Dashboard';
        vm.isBusy = true;
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

        vm.Branches = [];
        vm.ResourceFiles = [];
        vm.emailPattern = /^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}$/;
        vm.changeEmail = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                return datacontext.changeEmail(vm.Translator.Email).done(function (result) {
                    var scope = angular.element($("#dashboard-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                        });
                    } else {
                        vm.resultDialog("Error", result.Message);
                    }
                }).always(function () {
                    toggleSpinner(false);
                });
            }
            return false;
        }

        vm.setPassword = function () {
            window.location = "/Account/Manage";

        }

        vm.Download = function () {
            var url = "window.open('/Resources/For?branch=" + vm.currentBranch.Id + "&file=" + vm.currentResource.Id + "&culture=" + vm.currentLanguage.Culture + "','Download')";
            setTimeout(url, 10);
        }

        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }



        function activate() {
            var promises = [getDashboardInfo()];
            common.activateController(promises, controllerId)
                .then(function () { toggleSpinner(false); });
        }
        function getDashboardInfo() {
            return datacontext.getDashboardInfo().done(function (data) {
                return vm.Translator.Name = data.Name, vm.Translator.Languages = data.Languages, vm.Branches = data.Branches, vm.ResourceFiles = data.ResourceFiles, vm.Translator.Email = data.EmailAddress, vm.currentBranch = vm.Branches[0], vm.currentResource = vm.ResourceFiles[0], vm.currentLanguage = vm.Translator.Languages[0];
            }).fail(queryFailed);

        }

        function toggleSpinner(on) { vm.isBusy = on; }

        function queryFailed(error) {
            toggleSpinner(false);
            logError(error.message, "Query failed", true);
        }

        activate();
    }
})();