(function () {
    'use strict';

    var serviceId = 'datacontext';
    angular.module('app').factory(serviceId, ['common', datacontext]);

    function datacontext(common) {
        
        var breeze = window.breeze;
        breeze.config.initializeAdapterInstance("modelLibrary", "backingStore", true);
        var serviceName = '/breeze/AdminBreeze';
        var manager = new breeze.EntityManager(serviceName);
     
        var service = {
            getMissingTranslations: getMissingTranslations,
            getTranslators: getTranslators,
            getTranslator: getTranslator,
            saveNewTranslator: saveNewTranslator,
            editTranslator: editTranslator,
            sendPasswordMail: sendPasswordMail,
            sendTranslatorEmail: sendTranslatorEmail,
            getBranches: getBranches,
            getBranch: getBranch,
            editBranch: editBranch,
            saveNewBranch: saveNewBranch,
            removeBranch: removeBranch,
            getBranchesResourceFiles: getBranchesResourceFiles,
            getBranchResourceFile: getBranchResourceFile,
            getAllResourceFiles: getAllResourceFiles,
            getAllBranches: getAllBranches,
            editBranchResourceFile: editBranchResourceFile,
            saveNewBranchFile: saveNewBranchFile,
            removeBranchFile: removeBranchFile,
            getLanguages: getLanguages,
            removeLanguage: removeLanguage,
            saveNewLanguage: saveNewLanguage,
            getResourceFiles: getResourceFiles,
            getResourceFile: getResourceFile,
            editResourceFile: editResourceFile,
            saveNewResourceFile: saveNewResourceFile,
            removeResourceFile: removeResourceFile
        };

        return service;

        function getTranslators(currentPage, pageSize) {
            var skip = currentPage * pageSize;
           var query = breeze.EntityQuery
                   .from("Translators")
                   .orderBy("FirstName")
                   .skip(skip)
                   .take(pageSize)
                   .inlineCount(true);
           return manager.executeQuery(query);
        }

        function getAllResourceFiles() {
            var query = breeze.EntityQuery
                .from("ResourceFiles");
            return manager.executeQuery(query);
        }

        function getAllBranches() {
            var query = breeze.EntityQuery
                .from("Branches");
            return manager.executeQuery(query);
        }

        function getResourceFiles(currentPage, pageSize) {
            var skip = currentPage * pageSize;
            var query = breeze.EntityQuery
                    .from("ResourceFiles")
                    .orderBy("Id")
                    .skip(skip)
                    .take(pageSize)
                    .inlineCount(true);
            return manager.executeQuery(query);
        }

        function getResourceFile(id) {
            var query = breeze.EntityQuery
                .from("ResourceFiles")
                .where("Id", "eq", id * 1);

            return manager.executeQuery(query);
        }

        function getBranchesResourceFiles(currentPage, pageSize) {
            var skip = currentPage * pageSize;
            var query = breeze.EntityQuery
                    .from("BranchesResourceFiles")
                    .orderBy("Id")
                    .skip(skip)
                    .take(pageSize)
                    .inlineCount(true);
            return manager.executeQuery(query);
        }

        function getBranchResourceFile(id) {
            var query = breeze.EntityQuery
                .from("BranchesResourceFiles")
                .where("Id", "eq", id * 1);

            return manager.executeQuery(query);
        }

        function getLanguages(currentPage, pageSize) {
            var skip = currentPage * pageSize;
            var query = breeze.EntityQuery
                    .from("Languages")
                    .orderBy("Culture")
                    .skip(skip)
                    .take(pageSize)
                    .inlineCount(true);
            return manager.executeQuery(query);
        }

        function getBranches(currentPage, pageSize) {
            var skip = currentPage * pageSize;
            var query = breeze.EntityQuery
                    .from("Branches")
                    .orderBy("Id")
                    .skip(skip)
                    .take(pageSize)
                    .inlineCount(true);
            return manager.executeQuery(query);
        }

        function getBranch(id) {
            var query = breeze.EntityQuery
                .from("Branches")
                .where("Id", "eq", id * 1);
            return manager.executeQuery(query);
        }

        function editResourceFile(item) {
            return $.post("/Administration/AdministrationHome/EditResourceFile", { Id: item.Id, ResourceFileDisplayName: item.ResourceFileDisplayName, ResourceFileNameFormat: item.ResourceFileNameFormat });
        }

        function saveNewResourceFile(item) {
            return $.post("/Administration/AdministrationHome/CreateNewResourceFile", { Id: item.Id, ResourceFileDisplayName: item.ResourceFileDisplayName, ResourceFileNameFormat: item.ResourceFileNameFormat });
        }

        function removeResourceFile(item) {
            return $.post("/Administration/AdministrationHome/DeleteResourceFile", { Id: item.Id, ResourceFileDisplayName: item.ResourceFileDisplayName, ResourceFileNameFormat: item.ResourceFileNameFormat });
        }

        function editBranchResourceFile(item) {
            return $.post("/Administration/AdministrationHome/EditBranchResourceFile", { Id: item.Id, FK_BranchId: item.FK_BranchId, FK_ResourceFileId: item.FK_ResourceFileId, SyncRawPathAbsolute: item.SyncRawPathAbsolute });
        }

        function saveNewBranchFile(item) {
            return $.post("/Administration/AdministrationHome/CreateNewBranchResourceFile", { Id: item.Id, FK_BranchId: item.FK_BranchId, FK_ResourceFileId: item.FK_ResourceFileId, SyncRawPathAbsolute: item.SyncRawPathAbsolute });
        }

        function removeBranchFile(item) {
            return $.post("/Administration/AdministrationHome/DeleteBranchResourceFile", { Id: item.Id, FK_BranchId: item.FK_BranchId, FK_ResourceFileId: item.FK_ResourceFileId, SyncRawPathAbsolute: item.SyncRawPathAbsolute });
        }

        function editBranch(item) {
            return $.post("/Administration/AdministrationHome/EditBranch", { Id: item.Id, BranchDisplayName: item.BranchDisplayName, BranchRootUrl: item.BranchRootUrl });
        }

        function saveNewBranch(item) {
            return $.post("/Administration/AdministrationHome/CreateNewBranch", { Id: item.Id, BranchDisplayName: item.BranchDisplayName, BranchRootUrl: item.BranchRootUrl });
        }

        function removeBranch(item) {
            return $.post("/Administration/AdministrationHome/DeleteBranch", { Id: item.Id, BranchDisplayName: item.BranchDisplayName, BranchRootUrl: item.BranchRootUrl });
        }

        function saveNewLanguage(item) {
            return $.post("/Administration/AdministrationHome/CreateNewLanguage", { Culture: item.Culture, Description: item.Description });
        }

        function removeLanguage(item) {
            return $.post("/Administration/AdministrationHome/DeleteLanguage", { Culture: item.Culture, Description: item.Description });
        }

        function getTranslator(id) {
            var query = breeze.EntityQuery
                .from("Translators")
                .where("Id", "eq", id * 1);
                   
            return manager.executeQuery(query);
        }

        function saveNewTranslator(item) {
            return $.post("/Administration/AdministrationHome/CreateNewTranslator", { Id: item.Id, UserName: item.UserName, FirstName: item.FirstName,LastName: item.LastName,EmailAddress: item.EmailAddress,Cultures: item.Cultures, IsActive: item.IsActive, IsAdmin: item.IsAdmin });
        }

        function editTranslator(item) {
            return $.post("/Administration/AdministrationHome/EditTranslator", { Id: item.Id, UserName: item.UserName, FirstName: item.FirstName, LastName: item.LastName, EmailAddress: item.EmailAddress, Cultures: item.Cultures, IsActive: item.IsActive, IsAdmin: item.IsAdmin });
        }


        function sendPasswordMail(item) {
            return $.post("/Administration/AdministrationHome/SendPasswordEmail", { Id: item.Id, EmailAddress: item.EmailAddress });
        }

        function sendTranslatorEmail(item) {
            return $.post("/Administration/AdministrationHome/SendEmail", { Subject: item.Subject, Body: item.Content });
        }

        function getMissingTranslations(branchId) {
            return $.post("/Administration/AdministrationHome/GetMissingTranslations", { branchId: branchId });
        }
    }
})();