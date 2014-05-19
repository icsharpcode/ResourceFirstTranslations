(function () {
    'use strict';
    var controllerId = 'resetpassword';
    angular.module('app').controller(controllerId, ['common', 'datacontext', '$routeParams', '$location', 'bootstrap.dialog', resetpassword]);

    function resetpassword(common, datacontext, $routeParams, $location, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logError = getLogFn(controllerId, "error");
        var logSuccess = getLogFn(controllerId, "success");
        common.hideNavi();
        var vm = this;
        vm.title = 'Reset password';
        vm.currentTranslator = $routeParams.id;
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
        vm.Save = function() {
             toggleSpinner(true);
             return datacontext.sendPasswordMail(vm.item).done(function (result) {
                 var scope = angular.element($("#resetpassword-view")).scope();
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
        activate();
       
        function activate() {
            var promises = [getTranslator()];
            common.activateController(promises, controllerId)
                .then(function () { });
        }
       

        function getTranslator() {
            return datacontext.getTranslator(vm.currentTranslator).then(function (data) {
                data.results.forEach(function(item) {
                    item.Name = item.FirstName + " " + item.LastName;
                });
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
    }
})();