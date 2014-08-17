(function () {
    'use strict';
    var controllerId = 'translation';
    angular.module('app').controller(controllerId, ['common', '$rootScope', '$scope', 'datacontext', 'bootstrap.dialog', '$location', '$filter', translation]);

    function translation(common, $rootScope, $scope, datacontext, dialog, $location, $filter) {
        var logSuccess = common.logger.getLogFn(controllerId, "success");
        var logError = common.logger.getLogFn(controllerId, "error");
        common.hideNavi();
        var vm = this;
        vm.emptyTranslations = false;
        vm.modifiedTranslations = false;
        vm.resourceName = false;
        vm.enableMultiBranchTranslation = true;
        vm.resourceFileName = "";
        vm.resourceValue = false;
        vm.resourceFileValue = "";
        vm.translationValue = false;
        vm.translationFileValue = "";
        vm.additionalTranslationParameter = new Array();
        vm.additionalTranslation = [];
        vm.title = 'Filter';
        vm.resultTitle = 'Results';
        vm.editTitle = 'Edit translation';
        vm.editOtherBranches = 'Other Branches';
        vm.Branches = [];
        vm.Languages = [];
        vm.branchesCount = 0;
        vm.branchesFilteredCount = 0;
        vm.items = [];
        vm.busyMessage = 'Please wait ...';
        vm.showEditWidget = false;
        vm.originalItem = {};
        vm.editItem = {};
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
        vm.sort = {
            sortingOrder: 'ResourceIdentifier',
            reverse: false
        };
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 10
        };
        vm.pageChanged = pageChanged;
        Object.defineProperty(vm.paging, "pageCount", {
            get: function () {
                return Math.floor(vm.branchesFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        vm.sort_by = function (newSortingOrder) {
            var sort = vm.sort;
            if (sort.sortingOrder == newSortingOrder) {
                sort.reverse = !sort.reverse;
            }
            sort.sortingOrder = newSortingOrder;
        };

        vm.selectedCls = function (column) {

            if (column == vm.sort.sortingOrder) {
                return ('fa fa-chevron-' + ((vm.sort.reverse) ? 'down' : 'up'));
            }
            else {
                return 'fa fa-sort';
            }
        };

        vm.setDate = function (date) {
            return $filter('date')(date, 'yyyy/MM/dd');
        }

        vm.applyQuery = function () {
            vm.submitted = false;
            vm.editItem = {};
            vm.originalItem = {};
            closeAllPopover();
            vm.applyToAllBranches = false;
            vm.showEditWidget = false;
            toggleSpinner(true);
            return loadQuery();
        }

        vm.editTranslation = function (item) {
            vm.submitted = false;
            return copyItem(vm.editItem, item);
        }
        vm.closeEdit = function () {
            vm.editItem = {};
            closeAllPopover();
            vm.submitted = false;
            vm.originalItem = {};
            vm.showEditWidget = false;
        }
        vm.setIdentical = function (value) {
            return value == vm.originalItem.TranslatedValue;
        }

        vm.clearValues = function () {
            return vm.editItem = $.extend({}, vm.originalItem), vm.submitted = false, closeAllPopover();
        }

        vm.Save = function () {
            closeAllPopover();
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                if (vm.applyToAllBranches) {
                    var branchIds = [];
                    vm.originalItem.TranslatedValue = vm.editItem.TranslatedValue;
                    vm.editItem.Translations.forEach(function (e) {
                        e.TranslatedValue = vm.editItem.TranslatedValue;
                        branchIds.push(e.FK_BranchId);
                    });
                    vm.originalItem.Translations.forEach(function (e) {
                        e.TranslatedValue = vm.editItem.TranslatedValue;

                    });
                    return datacontext.editTranslationMultiBranch(vm.editItem, branchIds).done(function (result) {
                        var scope = angular.element($("#translation-view")).scope();
                        if (result.Succeeded) {
                            scope.$apply(function () {
                                logSuccess(result.Message);
                                vm.editItem.OriginalResxValueChangedSinceTranslation = result.OriginalResxValueChangedSinceTranslation;
                                vm.editItem.LastUpdated = $filter('date')(new Date(parseInt(result.LastUpdated.substr(6))), 'yyyy/MM/dd');
                                vm.editItem.LastUpdatedBy = result.LastUpdatedBy;
                                vm.originalItem.OriginalResxValueChangedSinceTranslation = result.OriginalResxValueChangedSinceTranslation;
                                vm.originalItem.LastUpdated = $filter('date')(new Date(parseInt(result.LastUpdated.substr(6))), 'yyyy/MM/dd');
                                vm.originalItem.LastUpdatedBy = result.LastUpdatedBy;
                                scope.vm.isBusy = false;
                            });
                        } else {
                            vm.resultDialog("Error", result.Message);
                        }
                    }).always(function () {
                        toggleSpinner(false);
                    });

                } else {
                    vm.originalItem.TranslatedValue = vm.editItem.TranslatedValue;
                    vm.editItem.Translations.forEach(function (e) {
                        if (e.Id == vm.editItem.Id) {
                            e.TranslatedValue = vm.editItem.TranslatedValue;
                        }
                    });
                    return datacontext.editTranslation(vm.editItem).done(function (result) {
                        var scope = angular.element($("#translation-view")).scope();
                        if (result.Succeeded) {
                            scope.$apply(function () {
                                logSuccess(result.Message);
                                vm.editItem.OriginalResxValueChangedSinceTranslation = result.OriginalResxValueChangedSinceTranslation;
                                vm.editItem.LastUpdated = $filter('date')(new Date(parseInt(result.LastUpdated.substr(6))), 'yyyy/MM/dd');
                                vm.editItem.LastUpdatedBy = result.LastUpdatedBy;
                                vm.originalItem.OriginalResxValueChangedSinceTranslation = result.OriginalResxValueChangedSinceTranslation;
                                vm.originalItem.LastUpdated = $filter('date')(new Date(parseInt(result.LastUpdated.substr(6))), 'yyyy/MM/dd');
                                vm.originalItem.LastUpdatedBy = result.LastUpdatedBy;
                                scope.vm.isBusy = false;
                            });
                        } else {
                            vm.resultDialog("Error", result.Message);
                        }
                    }).always(function () {
                        toggleSpinner(false);
                    });
                }
            }
            return false;
        }

        function activate() {
            var promises = [getTranslationFilterDefinition()];
            common.activateController(promises, controllerId)
                .then(function () {
                    toggleSpinner(true); loadQuery();
                });
        }

        function pageChanged(e) {
            vm.editItem = {};
            vm.originalItem = {};
            closeAllPopover();
            vm.applyToAllBranches = false;
            vm.showEditWidget = false;
            e && (vm.paging.currentPage = e, toggleSpinner(true) , loadQuery());
        }

        function getTranslationFilterDefinition() {
            return datacontext.getTranslationFilterDefinition().done(function (data) {
                return vm.Languages = data.Languages, vm.Branches = data.Branches, vm.enableMultiBranchTranslation = data.EnableMultiBranchTranslation, vm.currentLanguage = vm.Languages[0], vm.currentBranch = vm.Branches[0];
            });
        }

        function loadQuery() {
            common.activateController([getQueryResult()], controllerId)
                .then(function () {
                    if (vm.enableMultiBranchTranslation) {
                        fillAdditionalTranslations();
                    } else {
                        vm.applyToAllBranches = false;
                        toggleSpinner(false);
                    }
            });
        }

        function fillAdditionalTranslations() {
            vm.additionalTranslationParameter = [];
            if (vm.items.length > 0) {
                vm.items.forEach(function (item) {
                    vm.additionalTranslationParameter.push({ FileId: item.FK_ResourceFileId, ResourceId: item.ResourceIdentifier, Culture: item.Culture });
                });
                loadAdditionalTranslations(vm.additionalTranslationParameter);
            } else {
                toggleSpinner(false);
            }

        }

        function loadAdditionalTranslations(itemList) {
            common.activateController([getAdditionalTranslations(itemList)], controllerId)
                .then(function () { toggleSpinner(false); });
        }

        function getAdditionalTranslations(itemList) {
            return datacontext.getAdditionalTranslation(itemList).done(function (data) {
                vm.additionalTranslation = data;
                vm.items.forEach(function (item) {
                    item.Translations = vm.additionalTranslation.filter(function (e) { return e.ResourceIdentifier === item.ResourceIdentifier && e.FK_ResourceFileId === item.FK_ResourceFileId; });
                });

                return vm.items;
            });
        }

        function getQueryResult() {
            if (vm.currentBranch && vm.currentLanguage) {
                return datacontext.queryPerBranch(vm.paging.currentPage - 1, vm.paging.pageSize, vm.currentLanguage.Culture, vm.currentBranch.Id, vm.emptyTranslations, vm.modifiedTranslations, vm.resourceName, vm.resourceFileName, vm.resourceValue, vm.resourceFileValue, vm.translationValue, vm.translationFileValue).then(function(data) {
                    vm.items = data.results;
                    vm.items.forEach(function(item) {
                        item.Translations = [];
                    });
                    return vm.items, vm.branchesCount = data.inlineCount, vm.branchesFilteredCount = data.inlineCount;
                }).fail(queryFailed);
            } else {
                toggleSpinner(false);
            }
            return null;
        }

        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }

        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }

        function queryFailed(error) {
            toggleSpinner(false);
            logError(error.message, "Query failed", true);
        }

        function copyItem(destination, source) {
            vm.editItem = $.extend({}, source);
            vm.originalItem = source;
            if (vm.enableMultiBranchTranslation) {
                vm.applyToAllBranches = true;
            } else {
                vm.applyToAllBranches = false;
            }
           
            vm.originalItem.Translations.forEach(function (e) {
                if (e.TranslatedValue != vm.originalItem.TranslatedValue) {
                    vm.applyToAllBranches = false;
                    return;
                }
            });
            return vm.editItem, vm.originalItem, vm.applyToAllBranches, vm.showEditWidget = true;
        }

        function closeAllPopover() {
            $('span[id*=popover_]').each(function (n, e) {
                if ($(e).next().hasClass("popover")) {
                    var popover = angular.element($(e).next());
                    popover.scope().tt_isOpen = false;

                }
            });

        }

        activate();
    }
})();