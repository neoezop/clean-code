﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace Markdown
{
    [TestFixture]
    class MdTests
    {
        [SetUp]
        public void BaseSetup()
        {
            md = new Md();
        }

        private Md md;

        [Test]
        public void Render_ShouldThrowArgumentNullException_OnNullText()
        {
            Action act = () => md.Render(null);
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void AddElement_ShouldAddElemntCorrectly()
        {
            var mdElement = new MdElement('^', "<H1>", true);
            md.AddElement(mdElement);
            var expected = new MdElement('^', "<H1>", true);
            expected.HtmlTagClose = "<H1/>";
            md.elementSigns.ContainsKey('^').Should().BeTrue();
            md.elementSigns['^'].Should().BeEquivalentTo(expected);
        }

        [TestCase("", ExpectedResult = "")]
        [TestCase("_a_", ExpectedResult = "<em>a<em/>")]
        [TestCase("_a a a_", ExpectedResult = "<em>a a a<em/>")]
        [TestCase("*a*", ExpectedResult = "<strong>a<strong/>")]
        [TestCase("*a a a*", ExpectedResult = "<strong>a a a<strong/>")]
        [TestCase("a _a *a a", ExpectedResult = "a _a *a a")]
        [TestCase("a a_ a* a", ExpectedResult = "a a_ a* a")]
        [TestCase("_a_a_a_", ExpectedResult = "<em>a_a_a<em/>")]
        [TestCase("\\_a_", ExpectedResult = "_a_")]
        [TestCase("_a _a a_", ExpectedResult = "_a <em>a a<em/>")]
        [TestCase("_a _a_ a_", ExpectedResult = "<em>a <em>a<em/> a<em/>")]
        [TestCase("_a * a*_", ExpectedResult = "<em>a * a*<em/>")]
        [TestCase("*_ a _*", ExpectedResult = "*_ a _*")]
        public string Render_ShouldReturnCorrectResult(string text)
        {
            return md.Render(text);
        }
    }
}