﻿#region License

/*
    Copyright [2011] [Jeffrey Cameron]

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

#endregion

using System.Xml.Linq;
using Pickles.Parser;

namespace Pickles.DocumentationBuilders.HTML
{
    public class HtmlFeatureFormatter : IHtmlFeatureFormatter
    {
        private readonly HtmlDescriptionFormatter htmlDescriptionFormatter;
        private readonly HtmlImageResultFormatter htmlImageResultFormatter;
        private readonly HtmlScenarioFormatter htmlScenarioFormatter;
        private readonly HtmlScenarioOutlineFormatter htmlScenarioOutlineFormatter;
        private readonly XNamespace xmlns;

        public HtmlFeatureFormatter(
            HtmlScenarioFormatter htmlScenarioFormatter,
            HtmlDescriptionFormatter htmlDescriptionFormatter,
            HtmlScenarioOutlineFormatter htmlScenarioOutlineFormatter,
            HtmlImageResultFormatter htmlImageResultFormatter)
        {
            this.htmlScenarioFormatter = htmlScenarioFormatter;
            this.htmlScenarioOutlineFormatter = htmlScenarioOutlineFormatter;
            this.htmlDescriptionFormatter = htmlDescriptionFormatter;
            this.htmlImageResultFormatter = htmlImageResultFormatter;
            xmlns = HtmlNamespace.Xhtml;
        }

        #region IHtmlFeatureFormatter Members

        public XElement Format(Feature feature)
        {
            var div = new XElement(xmlns + "div",
                                   new XAttribute("id", "feature"),
                                   htmlImageResultFormatter.Format(feature),
                                   new XElement(xmlns + "h1", feature.Name),
                                   htmlDescriptionFormatter.Format(feature.Description)
                );

            var scenarios = new XElement(xmlns + "ul", new XAttribute("id", "scenarios"));
            int id = 0;

            if (feature.Background != null)
            {
                scenarios.Add(htmlScenarioFormatter.Format(feature.Background, id++));
            }

            foreach (IFeatureElement featureElement in feature.FeatureElements)
            {
                var scenario = featureElement as Scenario;
                if (scenario != null)
                {
                    scenarios.Add(htmlScenarioFormatter.Format(scenario, id++));
                }

                var scenarioOutline = featureElement as ScenarioOutline;
                if (scenarioOutline != null)
                {
                    scenarios.Add(htmlScenarioOutlineFormatter.Format(scenarioOutline, id++));
                }
            }

            div.Add(scenarios);

            return div;
        }

        #endregion
    }
}