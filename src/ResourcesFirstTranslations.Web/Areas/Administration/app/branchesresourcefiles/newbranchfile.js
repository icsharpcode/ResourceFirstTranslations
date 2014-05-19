(function () {
    'use strict';
    var controllerId = 'newbranchfile';
    angular.module('app').controller(controllerId, ['$scope', 'common', 'datacontext', '$location', 'bootstrap.dialog', newbranchfile]);

    function newbranchfile($scope, common, datacontext, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'New branch resource file';
        vm.Branches = [];
        vm.ResourceFiles = [];
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
            $location.path("/branchresourcefiles");
        }
        vm.Save = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                vm.item.FK_BranchId = vm.currentBranch.Id;
                vm.item.FK_ResourceFileId = vm.currentResource.Id;
                toggleSpinner(true);
                return datacontext.saveNewBranchFile(vm.item).done(function (result) {
                    var scope = angular.element($("#newbranchfile-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                            $location.path('/branchresourcefiles');
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
                FK_BranchId: 0,
                FK_ResourceFileId: 0,
                SyncRawPathAbsolute: ""
            }, vm.currentBranch = vm.Branches[0], vm.currentResource = vm.ResourceFiles[0], vm.submitted = false;
        }

        activate();

        function getAllBranches() {
            return datacontext.getAllBranches().then(function (data) {
                return vm.Branches = data.results;
            }).fail(queryFailed);
        }

        function getAllResourceFiles() {
            return datacontext.getAllResourceFiles().then(function (data) {
                return vm.ResourceFiles = data.results;
            }).fail(queryFailed);
        }

        function activate() {
            var promises = [getAllBranches(),getAllResourceFiles(), getBranchFile()];
            common.activateController(promises, controllerId)
                .then(function () {
                vm.currentBranch = vm.Branches[0]; vm.currentResource = vm.ResourceFiles[0]; });
        }
        function getBranchFile() {
            return vm.item = {
                Id: 0,
                FK_BranchId: 0,
                FK_ResourceFileId: 0,
                SyncRawPathAbsolute: ""
            };

        }
        
        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }

        function queryFailed(error) {
            logError(error.message, "Query failed", true);
        }
    }
})();