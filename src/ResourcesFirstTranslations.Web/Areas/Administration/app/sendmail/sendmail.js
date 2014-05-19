(function () {
    'use strict';
    var controllerId = 'sendmail';
    angular.module('app').controller(controllerId, ['$scope', 'common', 'datacontext', 'bootstrap.dialog', sendmail]);

    function sendmail($scope, common, datacontext, dialog) {
        var getLogFn = common.logger.getLogFn;
        var logSuccess = getLogFn(controllerId, "success");
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
        vm.SendMail = {
            Subject: "",
            Content: ""
        }
        vm.title = 'Send mail to all active translators';
        vm.Send = function () {
            if ($scope.myForm.$valid) {
                vm.submitted = false;
                toggleSpinner(true);
                return datacontext.sendTranslatorEmail(vm.SendMail).done(function(result) {
                    var scope = angular.element($("#sendmail-view")).scope();
                    if (result.Succeeded) {
                        scope.$apply(function () {
                            logSuccess(result.Message);
                            scope.vm.isBusy = false;
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
            return vm.SendMail.Subject = "", vm.SendMail.Content = "", vm.submitted = false;
        }
        activate();

        function activate() {
            common.activateController([], controllerId)
                .then(function () { });
        }

        function toggleSpinner(on) { vm.isBusy = on; }
        vm.resultDialog = function (title, message) {
            return dialog.resultDialog(title, message, "OK").then(function () { }).catch(function (e) { });
        }
    }
})();