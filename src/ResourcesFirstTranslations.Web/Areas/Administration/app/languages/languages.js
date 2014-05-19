(function () {
    'use strict';
    var controllerId = 'languages';
    angular.module('app').controller(controllerId, ['common', 'datacontext', 'bootstrap.dialog', '$location', languages]);

    function languages(common, datacontext, dialog, $location) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var removeItem = breeze.core.arrayRemoveItem;
        var vm = this;
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

        vm.title = 'Languages';
        vm.sort = {
            sortingOrder: 'Culture',
            reverse: false
        };
        vm.languagesCount = 0;
        vm.languagesFilteredCount = 0;
        vm.items = [];
        vm.paging = {
            currentPage: 1,
            maxPagesToShow: 5,
            pageSize: 10
        };
        vm.pageChanged = pageChanged;
        Object.defineProperty(vm.paging, "pageCount", {
            get: function () {
                return Math.floor(vm.languagesFilteredCount / vm.paging.pageSize) + 1;
            }
        });

        function pageChanged(e) {
            e && (vm.paging.currentPage = e, activate());
        }

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
       
        vm.removeLanguage = function (item) {
            return dialog.deleteDialog(item.Culture).then(function() {
                toggleSpinner(true);
                return datacontext.removeLanguage(item).done(function (result) {
                    var scope = angular.element($("#languages-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            removeItem(vm.items, item);
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
                        });
                    } else {
                        vm.resultDialog("Error", result.Message);
                    }
                }).always(function () {
                    toggleSpinner(false);

                });
            }).catch(function (e) { });
        }

        vm.createNew = function() {
            $location.path("/newlanguage");
        }


        activate();

        function activate() {
            common.activateController([getLanguages()], controllerId)
                .then(function () { });
        }

        function getLanguages() {
            return datacontext.getLanguages(vm.paging.currentPage - 1, vm.paging.pageSize).then(function (data) {
                return vm.items = data.results, vm.languagesCount = data.inlineCount, vm.languagesFilteredCount = data.inlineCount;
            }).fail(queryFailed);
        }

        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }

        function queryFailed(error) {
            toggleSpinner(false);
            logError(error.message, "Query failed", true);
        }
    }
})();