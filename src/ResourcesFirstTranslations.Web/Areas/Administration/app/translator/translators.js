(function () {
    'use strict';
    var controllerId = 'translators';
    angular.module('app').controller(controllerId, ['$location', 'common', 'datacontext', translators]);

    function translators($location,common, datacontext) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        common.hideNavi();
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
        vm.sort = {
            sortingOrder: 'FirstName',
            reverse: false
        };
        
       
        vm.attendeeCount = 0,
           vm.attendeeFilteredCount = 0,
           vm.items = [],
           vm.paging = {
               currentPage: 1,
               maxPagesToShow: 5,
               pageSize: 10
           },
          vm.pageChanged = pageChanged,
          vm.title = 'Translators';
           Object.defineProperty(vm.paging, "pageCount", {
               get: function () {
                   return Math.floor(vm.attendeeFilteredCount / vm.paging.pageSize) + 1;
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

        function activate() {
            var promises = [getTranslators()];
            common.activateController(promises, controllerId)
                .then(function () { });
        }

        function pageChanged(e) {
            e && (vm.paging.currentPage = e, activate());
        }

        vm.goToTranslator = function (e) {
            e && e.Id && $location.path("/translator/" + e.Id);
        }

        vm.resetPassword = function(e) {
            e && e.Id && $location.path("/resetpassword/" + e.Id);
        }

        vm.newTranslator = function() {
            $location.path("/newtranslator");
        }
        function getTranslators() {
            return datacontext.getTranslators(vm.paging.currentPage - 1, vm.paging.pageSize).then(function (data) {
                data.results.forEach(function(item) {
                    item.Name = item.FirstName + " " + item.LastName;
                });
                return vm.items = data.results, vm.attendeeCount = data.inlineCount, vm.attendeeFilteredCount = data.inlineCount;
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