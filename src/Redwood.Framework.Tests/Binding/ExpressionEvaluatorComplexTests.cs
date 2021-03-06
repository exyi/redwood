using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Redwood.Framework.Binding;
using Redwood.Framework.Controls;
using Redwood.Framework.Controls.Infrastructure;

namespace Redwood.Framework.Tests.Binding
{
    [TestClass]
    public class ExpressionEvaluatorComplexTests
    {

        /// <summary>
        /// Evaluates the binding with respect to DataContext which the target control is nested in.
        /// </summary>
        [TestMethod]
        public void ExpressionEvaluator_Complex_EvaluateInRootDataContext()
        {
            var view = new RedwoodView()
            {
                DataContext = new TestViewModel() { SubObject = new TestViewModel2() { Value = "hello" } },
                Children =
                {
                    new HtmlGenericControl("html")
                    {
                        Children =
                        {
                            new TextBox()
                            {
                                ID = "txb"    
                            }
                        }
                    }
                    .WithBinding(RedwoodBindableControl.DataContextProperty, new ValueBindingExpression("SubObject"))
                }
            };
            var textbox = view.FindControl("txb") as TextBox;

            var evaluator = new ExpressionEvaluator();
            var result = evaluator.Evaluate(new ValueBindingExpression("Value"), RedwoodBindableControl.DataContextProperty, textbox);

            Assert.IsInstanceOfType(result, typeof(string));
            Assert.AreEqual("hello", result);
        }

        /// <summary>
        /// Passes the value to the textbox which has a binding applied.
        /// </summary>
        [TestMethod]
        public void ExpressionEvaluator_UpdateSource_Simple()
        {
            var viewModel = new TestViewModel() { SubObject = new TestViewModel2() { Value = "hello" } };
            var view = new RedwoodView()
            {
                DataContext = viewModel,
                Children =
                {
                    new HtmlGenericControl("html")
                    {
                        Children =
                        {
                            new TextBox()
                            {
                                ID = "txb"    
                            }
                            .WithBinding(TextBox.TextProperty, new ValueBindingExpression("Value"))
                        }
                    }
                    .WithBinding(RedwoodBindableControl.DataContextProperty, new ValueBindingExpression("SubObject"))
                }
            };
            var textbox = view.FindControl("txb") as TextBox;
            var binding = textbox.GetBinding(TextBox.TextProperty) as ValueBindingExpression;

            binding.UpdateSource("test", textbox, TextBox.TextProperty);

            Assert.AreEqual("test", viewModel.SubObject.Value);
        }


        class TestViewModel
        {
            public TestViewModel2 SubObject { get; set; }
        }

        class TestViewModel2
        {
            public string Value { get; set; }
        }
    }
}