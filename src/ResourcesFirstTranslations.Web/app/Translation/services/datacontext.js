(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId, ['common', datacontext]);

    function datacontext(common) {
        var $q = common.$q;

        var breeze = window.breeze;
        var getLogFn = common.logger.getLogFn;
        var log = getLogFn(serviceId);
        breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
        var serviceName = '/breeze/Breeze';
        var manager = new breeze.EntityManager(serviceName);
        var service = {
            getDashboardInfo: getDashboardInfo,
            changeEmail: changeEmail,
            queryPerBranch: queryPerBranch,
            getTranslationFilterDefinition: getTranslationFilterDefinition,
            getAdditionalTranslation: getAdditionalTranslation,
            editTranslation: editTranslation,
            editTranslationMultiBranch: editTranslationMultiBranch
        };

        return service;

        

        function getAdditionalTranslation(itemsList) {
            var data = {
                items: itemsList
            };
            return $.ajax({
                url: 'Translation/GetAdditionalTranslations',
                type: 'POST',
                data: JSON.stringify(data) ,
                contentType: 'application/json; charset=utf-8'
            }); 
        }

        function queryPerBranch(currentPage, pageSize, language, branch, empty, modified, resourceName, resourceNameText, resourceValue, resourceValueText, translationValue, translationValueText) {
            var skip = currentPage * pageSize;
            var p1 = breeze.Predicate("Culture", "eq", language);
            var p2 = breeze.Predicate("FK_BranchId", "eq", branch);
            var p3 = breeze.Predicate("TranslatedValue", "eq", null);
            var p4 = breeze.Predicate("TranslatedValue", "eq", "");
            var p5 = breeze.Predicate("OriginalResxValueChangedSinceTranslation", "eq", true);
            var p6 = breeze.Predicate("ResourceIdentifier", "contains", resourceNameText);
            var p7 = breeze.Predicate("ResxValue", "contains", resourceValueText);
            var p8 = breeze.Predicate("TranslatedValue", "contains", translationValueText);
            var predicate = p1.and(p2);
            if (empty && !modified && !resourceName && !resourceValue && !translationValue) {
               
                predicate = p1.and(p2.and(p3.or(p4)));
            }
            else if (!empty && modified && !resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p5));
            }
            else if (!empty && !modified && resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p6));
            }
            else if (!empty && !modified && !resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p7));
            }
            else if (!empty && !modified && !resourceName && !resourceValue && translationValue) {
                predicate = p1.and(p2.and(p8));
               
            }
            else if (empty && modified && resourceName && resourceValue && translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p5.and(p6.and(p7.and(p8))))));
            }
            else if (empty && modified && resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p5.and(p6.and(p7)))));
            }
            else if (empty && modified && resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p5.and(p6))));
            }
            else if (empty && modified && !resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p5)));
            }
            else if (!empty && modified && resourceName && resourceValue && translationValue) {
                predicate = p1.and(p2.and(p5.and(p6.and(p7.and(p8)))));
            }
            else if (!empty && modified && resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p5.and(p6.and(p7))));
            }
            else if (!empty && modified && resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p5.and(p6)));
            }
            else if (!empty && !modified && resourceName && resourceValue && translationValue) {
                predicate = p1.and(p2.and(p6.and(p7.and(p8))));
            }
            else if (!empty && !modified && resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p6.and(p7)));
            }
            else if (!empty && !modified && !resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p7.and(p8)));
            }
            else if (!empty && modified && !resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p5.and(p7)));
            }
            else if (!empty && modified && !resourceName && !resourceValue && translationValue) {
                predicate = p1.and(p2.and(p5.and(p8)));
            }
            else if (!empty && !modified && resourceName && !resourceValue && translationValue) {
                predicate = p1.and(p2.and(p6.and(p8)));
            }
            else if (empty && !modified && !resourceName && !resourceValue && translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p8)));
            }
            else if (empty && !modified && !resourceName && resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p7)));
            }
            else if (empty && !modified && resourceName && !resourceValue && !translationValue) {
                predicate = p1.and(p2.and(p3.or(p4).and(p6)));
            }
            var query = breeze.EntityQuery
                    .from("QueryPerBranch")
                    .where(predicate)
                    .orderBy("Id")
                    .skip(skip)
                    .take(pageSize)
                    .inlineCount(true);
            return manager.executeQuery(query);
        }

        function getTranslationFilterDefinition() {
            return $.post("Translation/GetTranslationFilterDefinition");

        }

        function getDashboardInfo() {
            return $.post("Translation/GetDashboardInfo");
        }
        function changeEmail(newEmailAddress) {
            return $.post("Translation/ChangeEmailAddress", { newEmailAddress: newEmailAddress });
        }
        function editTranslation(item) {
            return $.post("Translation/EditTranslation", { Id: item.Id, TranslatedValue: item.TranslatedValue });
        }
        function editTranslationMultiBranch(item, branchIds) {
            var data = {
                Id: item.Id,
                TranslatedValue: item.TranslatedValue,
                branchIds: branchIds
            };
            return $.ajax({
                url: 'Translation/EditTranslationMultiBranch',
                type: 'POST',
                data: JSON.stringify(data),
                contentType: 'application/json; charset=utf-8'
            });
        }

       
    }
})();