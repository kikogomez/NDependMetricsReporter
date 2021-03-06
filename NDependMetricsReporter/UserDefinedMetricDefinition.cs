﻿using System.Xml.Serialization;
using System.Collections.Generic;

namespace NDependMetricsReporter
{
    [System.Serializable()]
    public class UserDefinedMetricDefinition
    {
        private string nDependCodeElementTypeField;
        private string metricTypeField;
        private string resumedMetricNameField;
        private string methodNameToInvokeField;
        private string metricNameField;
        private string descriptionField;
        private List<string> codeSectionsField;

        [XmlElement("NDependCodeElementType")]
        public string NDependCodeElementType
        {
            get { return this.nDependCodeElementTypeField; }
            set { this.nDependCodeElementTypeField = value; }
        }

        [XmlElement("MetricType")]
        public string MetricType
        {
            get { return this.metricTypeField; }
            set { this.metricTypeField = value; }
        }

        [XmlElement("ResumedMetricName")]
        public string ResumedMetricName
        {
            get { return this.resumedMetricNameField; }
            set { this.resumedMetricNameField = value; }
        }

        [XmlElement("MethodNameToInvoke")]
        public string MethodNameToInvoke
        {
            get { return this.methodNameToInvokeField; }
            set { this.methodNameToInvokeField = value; }
        }

        [XmlElement("MetricName")]
        public string MetricName
        {
            get { return this.metricNameField; }
            set { this.metricNameField = value; }
        }

        [XmlElement("Description")]
        public string Description
        {
            get { return this.descriptionField; }
            set { this.descriptionField = value; }
        }

        [XmlArray("CodeSections")]
        [XmlArrayItem("CodeSection")]
        public List<string> CodeSections
        {
            get { return this.codeSectionsField; }
            set { this.codeSectionsField = value; }
        }
    }
}
