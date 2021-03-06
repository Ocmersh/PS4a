﻿// Written by Joe Zachary for CS 3500, January 2017.
// Additional tests written by Bryce Hansen, Feb. 2018.

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct4()
        {
            Formula f = new Formula("2+3)");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("((2+3)");
        }

        /// <summary>
        /// Empty formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct6()
        {
            Formula f = new Formula("");
        }

        /// <summary>
        /// Invalid first
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("%((2+3))");
        }

        /// <summary>
        /// Invalid last
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct8()
        {
            Formula f = new Formula("((2+3))%");
        }

        /// <summary>
        /// two var with no operator
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("x y");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5 ()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        [TestMethod]
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate6()
        {
            Formula f = new Formula("5/0");
            Assert.AreEqual(f.Evaluate(v => 0), 0, 1e-6);
        }

        /// <summary>
        /// test null formula argument
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullForm1()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void GetVar1()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            string result = "";

            foreach (var token in f.GetVariable())
            {
                result += token;
            }

            Assert.AreEqual("xyz", result);
        }

        /// <summary>
        /// Checks accuracy of toString method
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            string result = f.ToString();

            Assert.AreEqual("(x+y)*(z/x)*1.0", result);

        }

        /// <summary>
        /// simple Normalizer that converts a string to uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string Normalizer(string s)
        {
            return s.ToUpper();
        }

        /// <summary>
        /// empty Normalizer that does nothing
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string Normalizer2(string s)
        {
            return s;
        }

        /// <summary>
        /// simple Validator that verifies if a string is uppercase
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool Validator(string s)
        {
            if (s.ToUpper().Equals(s))
                return true;
            else
                return false;
        }

        /// <summary>
        /// Verifies the output of the normalizer
        /// </summary>
        [TestMethod]
        public void NormTest()
        {
            Formula t = new Formula("x+y", Normalizer, Validator);

            Assert.AreEqual("X+Y", t.ToString());
        }

        /// <summary>
        /// Tests the creation of formula with a valid normalizer
        /// </summary>
        [TestMethod]
        public void ValidTest()
        {
            Formula t = new Formula("x+y", Normalizer, Validator);

            Assert.AreEqual(true, Validator(t.ToString()));
        }

        /// <summary>
        /// Tests the creation of formula with an invalid normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidTest2()
        {
            Formula t = new Formula("x+y", Normalizer2, Validator);
        }

        /// <summary>
        /// Tests the creation of formula with a valid normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ValidTest3()
        {
            Formula t = new Formula("x+y", Normalizer, null);
        }

        /// <summary>
        /// Test to see if normalized formula is equivalent to a non-normalized one.
        /// </summary>
        [TestMethod]
        public void Equivalent()
        {
            Formula a = new Formula("A+B");
            Formula b = new Formula("a+b", Normalizer, Validator);
            Assert.AreEqual(a.ToString(), b.ToString());
        }



    }
}
