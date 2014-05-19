(function () {
    'use strict';
    var controllerId = 'newresourcefile';
    angular.module('app').controller(controllerId, ['$scope', 'common', 'datacontext', '$location', 'bootstrap.dialog', newresourcefile]);

    function newresourcefile($scope, common, datacontext, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'New resource file';
        vm.minId = 1;
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
            $location.path("/resourcefiles");
        }
        vm.Save = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                return datacontext.saveNewResourceFile(vm.item).done(function(result) {
                    var scope = angular.element($("#newresourcefile-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                            $location.path('/resourcefiles');
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
                ResourceFileDisplayName: "",
                ResourceFileNameFormat: ""
               
            }, vm.submitted = false;
        }

        function activate() {
            var promises = [getResourceFile()];
            common.activateController(promises, controllerId)
                .then(function () { });
        }
        function getResourceFile() {
            return vm.item = {
                Id: 0,
                ResourceFileDisplayName: "",
                ResourceFileNameFormat: ""
              
            };

        }
        
        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }
        activate();
    }
})();