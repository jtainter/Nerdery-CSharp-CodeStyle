using System;
using StyleCop;
using StyleCop.CSharp;

namespace NerderyExtensions.Stylecop.Rules
{
    #region Content

    /// <summary>
    /// This stylecop rule will require Public methods to be documented
    /// </summary>
    [SourceAnalyzer(typeof (CsParser))]
    public class NerderyCustomRules : SourceAnalyzer
    {
        //public StreamWriter _logFile;
        /// <summary>
        /// This method will analyze the document and call the routine that 
        /// will make the determination whether this rule has been violated
        /// </summary>
        /// <param name="document"></param>
        public override void AnalyzeDocument(CodeDocument document)
        {
            //_logFile = new StreamWriter(@"C:\\Logs\HSM\sytlelog.txt");
            var csdocument = (CsDocument)document;

            if (csdocument.RootElement != null && !csdocument.RootElement.Generated)
            {
                csdocument.WalkDocument(PublicConstructorShouldBeDocumented, null, null);
                csdocument.WalkDocument(PublicMethodsShouldBeDocumented, null, null);
                csdocument.WalkDocument(RegionMustBeSeparatedByBlankLine, null, null);
                csdocument.WalkDocument(RegionNameMustHaveFirstLetterCapitalized, null, null);
            }

            //_logFile.Close();
        }

        private bool PublicConstructorShouldBeDocumented(CsElement element, CsElement parentElement, object context)
        {
            if (!element.Generated && element.ElementType == ElementType.Constructor && element.ActualAccess == AccessModifierType.Public)
            {
                if ((element.Header == null) || (element.Header.Text.Length == 0))
                {
                    AddViolation(element, "PublicConstructorsShouldBeDocumented");
                }
            }

            return true;
        }

        private bool PublicMethodsShouldBeDocumented(CsElement element, CsElement parentElement, object context)
        {
            if (!element.Generated && element.ElementType == ElementType.Method && element.ActualAccess == AccessModifierType.Public)
            {
                if ((element.Header == null) || (element.Header.Text.Length == 0))
                {
                    AddViolation(element, "PublicMethodsShouldBeDocumented");
                }
            }

            return true;
        }

        private bool RegionNameMustHaveFirstLetterCapitalized(CsElement element, CsElement parentElement, object context)
        {
            if (element.ElementType == ElementType.Root)
            {
                //_logFile.WriteLine("Start Processing Document Root");
                for (Node<CsToken> node = element.Tokens.First; node != element.Tokens.Last.Next; node = node.Next)
                {
                    //_logFile.WriteLine(string.Format("Node: {0}", node.Value));
                    if (node.Value.CsTokenClass == CsTokenClass.RegionDirective)
                    {
                        var region = (Region)node.Value;
                        if (region.Text.Contains("#region"))
                        {
                            var regionText = region.Text.Split(' ');
                            if (regionText.Length <= 1)
                            {
                                AddViolation(element, node.Value.LineNumber, "RegionNameMustHaveFirstLetterCapitalized");
                            }
                            else
                            {
                                if (!regionText[1].HasValue())
                                {
                                    AddViolation(element, node.Value.LineNumber, "RegionNameMustHaveFirstLetterCapitalized");
                                }
                                else
                                {
                                    var firstChar = regionText[1].Trim().ToCharArray();
                                    if (firstChar.Length <= 1 || !char.IsUpper(firstChar[0]))
                                    {
                                        AddViolation(element, node.Value.LineNumber, "RegionNameMustHaveFirstLetterCapitalized");
                                    }
                                }

                            }
                        }
                    }

                }
            }

            return true;
        }

        private bool RegionMustBeSeparatedByBlankLine(CsElement element, CsElement parentElement, object context)
        {
            if (element.ElementType == ElementType.Root)
            {
                //_logFile.WriteLine("Start Processing Document Root");
                for (Node<CsToken> node = element.Tokens.First; node != element.Tokens.Last.Next; node = node.Next)
                {
                    //_logFile.WriteLine(string.Format("Node: {0}", node.Value));
                    if (node.Value.CsTokenClass == CsTokenClass.RegionDirective)
                    {
                        var region = (Region)node.Value;
                        if (region.Text.Contains("#region"))
                        {
                            var isSpaceBeforeValid = RegionStartCheckIfBlankOrCurly(element.Document.SourceCode, node.Value.LineNumber - 1);
                            if (!isSpaceBeforeValid)
                                AddViolation(element, node.Value.LineNumber, "RegionMustBeSeparatedByBlankLine");

                            isSpaceBeforeValid = RegionStartCheckIfBlankOrCurly(element.Document.SourceCode, node.Value.LineNumber + 1, false);
                            if (!isSpaceBeforeValid)
                                AddViolation(element, node.Value.LineNumber, "RegionMustBeSeparatedByBlankLine");
                        }

                        if (region.Text.Contains("#endregion"))
                        {
                            var isSpaceAfterValid = RegionEndCheckIfBlankOrCurly(element.Document.SourceCode, node.Value.LineNumber - 1, false);
                            if (!isSpaceAfterValid)
                                AddViolation(element, node.Value.LineNumber, "RegionMustBeSeparatedByBlankLine");

                            isSpaceAfterValid = RegionEndCheckIfBlankOrCurly(element.Document.SourceCode, node.Value.LineNumber + 1);
                            if (!isSpaceAfterValid)
                                AddViolation(element, node.Value.LineNumber, "RegionMustBeSeparatedByBlankLine");
                        }
                    }

                }
            }

            return true;
        }

        #region Helper Methods

        private bool RegionStartCheckIfBlankOrCurly(SourceCode codeFile, int linenumber, bool allowCurly = true)
        {
            var myReader = codeFile.Read();
            for (var i = 1; i <= linenumber; i++)
            {
                var data = myReader.ReadLine();
                if (i == linenumber)
                {
                    //_logFile.WriteLine("LineNumber: {0} -- Text: {1}", linenumber, data);

                    if (data.Trim() == string.Empty || (data.Trim().Contains("{") && allowCurly)) return true;
                }
            }

            return false;
        }

        private bool RegionEndCheckIfBlankOrCurly(SourceCode codeFile, int linenumber, bool allowCurly = true)
        {
            var myReader = codeFile.Read();
            for (var i = 1; i <= linenumber; i++)
            {
                var data = myReader.ReadLine();
                if (i == linenumber)
                {
                    //_logFile.WriteLine("LineNumber: {0} -- Text: {1}", linenumber, data);

                    if (data.Trim() == string.Empty || (data.Trim().Contains("}") && allowCurly)) return true;
                }
            }

            return false;
        }

        #endregion;
    }

    #endregion;
}