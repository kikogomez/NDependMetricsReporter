﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NDepend;
using NDepend.Path;
using NDepend.Analysis;
using NDepend.Project;
using NDepend.CodeModel;
using NDepend.PowerTools;

namespace NDependMetricsReporter
{
    class AnalysisHistoryManager
    {
        List<IAnalysisResultRef> analysisResultRefsList;
        public AnalysisHistoryManager(string nDpendProjectpath)
        {
            NDependServicesProvider nDependServicesProvider = new NDependServicesProvider();
            var projectManager = nDependServicesProvider.ProjectManager;
            IAbsoluteFilePath pathToNDependProject = PathHelpers.ToAbsoluteFilePath(nDpendProjectpath);
            IProject nDependProjet = projectManager.LoadProject(pathToNDependProject);
            ICollection<IAnalysisResultRef> analysisResultRefs = nDependProjet.GetAvailableAnalysisResultsRefs();
            analysisResultRefsList = analysisResultRefs.OrderBy(analysisResultRef => analysisResultRef.Date).ToList();
        }

        public AnalysisHistoryManager(IProject nDependProject)
        {
            ICollection<IAnalysisResultRef> analysisResultRefs = nDependProject.GetAvailableAnalysisResultsRefs();
            analysisResultRefsList = analysisResultRefs.OrderBy(analysisResultRef => analysisResultRef.Date).ToList();
        }

        public List<IAnalysisResultRef> AnalysisResultRefsList
        {
            get { return analysisResultRefsList; }
        }

        public IList GetMetricHistory(object codeElement, NDependMetricDefinition metricDefinition)
        {
            CodeElementsManagerReflectionHelper reflectionHelper = new CodeElementsManagerReflectionHelper();
            Type metricType = Type.GetType(metricDefinition.NDependMetricType);
            Type nullableMetricType = typeof(Nullable<>).MakeGenericType(metricType);
            IList metricValues = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(nullableMetricType));
            var metricValue = Activator.CreateInstance(nullableMetricType);

            foreach (var m in analysisResultRefsList)
            {
                try
                {
                    IAnalysisResult currentAnalysisResult = m.Load();
                    ICodeBase currentAnalysisResultCodeBase = currentAnalysisResult.CodeBase;
                    CodeElementsManager currenAnalysisResultCodeBaseManager =  new CodeElementsManager(currentAnalysisResultCodeBase);
                    //var metricValue = Convert.ChangeType(0, metricType);
                    //var metricValue = Convert.ChangeType(0, nullableMetricType);
                    metricValue = null;

                    switch (metricDefinition.NDependCodeElementType)
                    {
                        case "NDepend.CodeModel.IAssembly":
                            string assemblyName = ((IAssembly)codeElement).Name;
                            IAssembly selectedAssemblyFromCurrentAnalysisResultCodebase = currenAnalysisResultCodeBaseManager.GetAssemblyByName(assemblyName);
                            if (selectedAssemblyFromCurrentAnalysisResultCodebase != null)
                                metricValue = reflectionHelper.GetCodeElementMetric(
                                    selectedAssemblyFromCurrentAnalysisResultCodebase,
                                    typeof(IAssembly),
                                    metricDefinition.InternalPropertyName,
                                    metricDefinition.NDependMetricType);
                            break;

                        case "NDepend.CodeModel.INamespace":
                            string namespaceName = ((INamespace)codeElement).Name;
                            INamespace selectedNamespaceFromCurrentAnalysisResultCodebase = currenAnalysisResultCodeBaseManager.GetNamespaceByName(namespaceName);
                            if (selectedNamespaceFromCurrentAnalysisResultCodebase != null)
                                metricValue = reflectionHelper.GetCodeElementMetric(
                                    selectedNamespaceFromCurrentAnalysisResultCodebase,
                                    typeof(INamespace),
                                    metricDefinition.InternalPropertyName,
                                    metricDefinition.NDependMetricType);
                            break;
                        case "NDepend.CodeModel.IType":
                            string typeName = ((IType)codeElement).Name;
                            IType selectedTypeFromCurrentAnalysisResultCodebase = currenAnalysisResultCodeBaseManager.GetTypeByName(typeName);
                            if (selectedTypeFromCurrentAnalysisResultCodebase != null)
                                metricValue = reflectionHelper.GetCodeElementMetric(
                                    selectedTypeFromCurrentAnalysisResultCodebase,
                                    typeof(IType),
                                    metricDefinition.InternalPropertyName,
                                    metricDefinition.NDependMetricType);
                            break;
                        case "NDepend.CodeModel.IMethod":
                            string methodName = ((IMethod)codeElement).Name;
                            IMethod selectedMethodFromCurrentAnalysisResultCodebase = currenAnalysisResultCodeBaseManager.GetMethodByName(methodName);
                            if (selectedMethodFromCurrentAnalysisResultCodebase != null)
                                metricValue = (reflectionHelper.GetCodeElementMetric(
                                    selectedMethodFromCurrentAnalysisResultCodebase,
                                    typeof(IMethod),
                                    metricDefinition.InternalPropertyName,
                                    metricDefinition.NDependMetricType));
                            break;
                    }
                    metricValues.Add(metricValue);
                }
                catch (AnalysisException analysisException)
                {
                    string exceptionString = analysisException.ToString();
                }
            }
            return metricValues;
        }

    }
}
