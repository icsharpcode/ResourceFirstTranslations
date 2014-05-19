(function () {
    'use strict';
    var controllerId = 'editbranchfile';
    angular.module('app').controller(controllerId, ['$scope', '$rootScope', 'common', 'datacontext', '$routeParams', '$location', 'bootstrap.dialog', editbranchfile]);

    function editbranchfile($scope, $rootScope, common, datacontext, $routeParams, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.back = false;
        vm.backUrl = "/branchresourcefiles";
        var onRouteChangeOff;
        vm.title = 'Edit branch resource file';
        vm.currentBrancheFile = $routeParams.id;
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
            $location.path(vm.backUrl);
        }

        vm.Save = function () {
            if ($scope.myForm.$valid) {
                toggleSpinner(true);
                vm.submitted = false;
                vm.editItem.FK_BranchId = vm.currentBranch.Id;
                vm.editItem.FK_ResourceFileId = vm.currentResource.Id;
                vm.item = $.extend({}, vm.editItem);
                datacontext.editBranchResourceFile(vm.item).done(function (result) {
                    var scope = angular.element($("#editbranchfile-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                            onRouteChangeOff(); //Stop listening for location changes
                            $location.path('/branchresourcefiles');
                        });
                    } else {
                        vm.resultDialog("Error", result.Message);
                    }
                }).always(function () {
                    toggleSpinner(false);
                });
            }

        }

        vm.isItemUnchanged = function () {
            vm.editItem.FK_BranchId = vm.currentBranch.Id;
            vm.editItem.FK_ResourceFileId = vm.currentResource.Id;
            if (!angular.equals(vm.editItem.FK_BranchId, vm.item.FK_BranchId)) {
                return false;
            }
            if (!angular.equals(vm.editItem.FK_ResourceFileId, vm.item.FK_ResourceFileId)) {
                return false;
            }
            if (!angular.equals(vm.editItem.SyncRawPathAbsolute, vm.item.SyncRawPathAbsolute)) {
                return false;
            }
            if (!angular.equals(vm.editItem.Branch, vm.item.Branch)) {
                return false;
            }
            if (!angular.equals(vm.editItem.ResourceFile, vm.item.ResourceFile)) {
                return false;
            }
            return true;

        };

        vm.clearValues = function () {
            vm.currentBranch = setDropDownValues(vm.Branches, vm.item.FK_BranchId);
            vm.currentResource = setDropDownValues(vm.ResourceFiles, vm.item.FK_ResourceFileId);
            return vm.editItem = $.extend({}, vm.item), vm.currentBranch, vm.currentResource, vm.submitted = false;
        };

        function activate() {
            var promises = [getAllBranches(), getAllResourceFiles(), getBranchResourceFile()];
            common.activateController(promises, controllerId)
                .then(function () {
                    vm.editItem = $.extend({}, vm.item);
                    vm.currentBranch = setDropDownValues(vm.Branches, vm.editItem.FK_BranchId);
                    vm.currentResource = setDropDownValues(vm.ResourceFiles, vm.editItem.FK_ResourceFileId);
                    onRouteChangeOff = $rootScope.$on('$locationChangeStart', routeChange);
                });
        }

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

        function getBranchResourceFile() {
            return datacontext.getBranchResourceFile(vm.currentBrancheFile).then(function (data) {
                return vm.item = data.results[0];
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

        function setDropDownValues(input, id) {
            var value = getById(input, id);
            if (null == value) {
                value = input[0];;
            }
            return value;

        }

        function getById(input, id) {
            var i = 0, len = input.length;
            for (; i < len; i++) {
                if (+input[i].Id == +id) {
                    return input[i];
                }
            }
            return null;
        };

        activate();
    }

})();