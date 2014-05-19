(function () {
    'use strict';
    var controllerId = 'dashboard';
    angular.module('app').controller(controllerId, ['$rootScope','$location', 'common', 'datacontext', dashboard]);

    function dashboard($rootScope,$location, common, datacontext) {
        common.hideNavi();
        var vm = this;
        vm.title = 'Missing translations';
        vm.Branches = [];
        vm.busyMessage = 'Please wait ...';
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
    
        vm.MissingTranslations = [];
        vm.update = function () {
            if (vm.Branch) {
                toggleSpinner(true);
                return loadMissingTranslations(vm.Branch.Id);
                
            }
        }
        activate();
       
        function activate() {
            var promises = [getAllBranches()];
            common.activateController(promises, controllerId)
                .then(function () {
                    vm.Branch = vm.Branches[0];
                if (vm.Branch) {
                    loadMissingTranslations(vm.Branch.Id);
                }
                toggleSpinner(false);
            });
          
        }

        function loadMissingTranslations(branchId) {
            var promises = [getMissingTranslations(branchId)];
            common.activateController(promises, controllerId)
                .then(function () {
                    toggleSpinner(false);
                });
        }

        function getMissingTranslations(branchId) {
            return datacontext.getMissingTranslations(branchId).done(function (data) {
                return vm.MissingTranslations = data;
            });
        }
        function getAllBranches() {
            return datacontext.getAllBranches().then(function (data) {
                return vm.Branches = data.results;
            });
        }

        function toggleSpinner(on) { vm.isBusy = on; }
       
    }
})();