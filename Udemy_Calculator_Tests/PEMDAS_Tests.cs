﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using Udemy_Calculator;

namespace Udemy_Calculator_Tests
{

    [TestClass]
    public class PEMDAS_Tests
    {
        #region Test on parenthesis

        [TestMethod]
        public void ParenthesisEquivalence_BadNumberParenthesisOpenedAndClosed_ReturnsNotCompleted()
        {
            // Arrange
            StringBuilder lFormula = new StringBuilder("4*((5/2)");

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula.ToString());
            bool lboolParenthesis = lPEMDAS.ParenthesisAreEquivalent(lFormula);

            // Assert
            Assert.IsFalse(lboolParenthesis);
        }

        [TestMethod]
        public void ParenthesisEquivalence_GoodNumberParenthesisOpenedAndClosed_ReturnsCompleted()
        {
            // Arrange
            StringBuilder lFormula = new StringBuilder("4*((5/2))");

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula.ToString());
            bool lboolParenthesis = lPEMDAS.ParenthesisAreEquivalent(lFormula);

            // Assert
            Assert.IsTrue(lboolParenthesis);
        }

        [TestMethod]
        public void IsRealOpenedParenthesis_NotOpenedParenthesis_ReturnsTrue()
        {
            // Arrange
            string lFormula = "(5+2)";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 0, lFormula.Length);

            // Assert
            Assert.IsTrue(lPEMDAS.IsRealOpenedParenthesis(lPEMDAS.Chunk)); // GOOD '('
        }

        [TestMethod]
        public void IsRealOpenedParenthesis_RealOpenedParenthesis_ReturnsTrue()
        {
            // Arrange
            string lFormula = "4*((5/2))";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 2, lFormula.Length);

            // Assert
            Assert.IsTrue(lPEMDAS.IsRealOpenedParenthesis(lPEMDAS.Chunk)); // GOOD '('
        }

        [TestMethod]
        public void IsRealOpenedParenthesis_ClosedParenthesis_ReturnsFalse()
        {
            // Arrange
            string lFormula = "4*((5/2))";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 7, lFormula.Length);

            // Assert
            Assert.IsFalse(lPEMDAS.IsRealOpenedParenthesis(lPEMDAS.Chunk)); // BAD ')'
        }

        [TestMethod]
        public void IsRealOpenedParenthesis_ParenthesisPreceedByExponentSymbol_ReturnsFalse()
        {
            // Arrange
            string lFormula = "2^(5/3)+4*((5/2))";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 2, lFormula.Length);

            // Assert
            Assert.IsFalse(lPEMDAS.IsRealOpenedParenthesis(lPEMDAS.Chunk)); // BAD '^('
        }

        [TestMethod]
        public void ComputeParenthesis_PutFormulaWithParenthesis_ReturnsChunkWithTheSmallerParenthesis()
        {
            // Arrange
            string lFormula = "2^(5/3)+4*((5/2))";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 0, lFormula.Length);
            lPEMDAS.ComputeParenthesis();

            // Assert
            Assert.AreEqual("(5/2)", lPEMDAS.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeParenthesis_PutFormulaWithParenthesis_ReturnsStartIndexAndLength()
        {
            // Arrange
            string lFormula = "2^(5/3)+4*((5/2))";

            // Act
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.Chunk = new Chunk(new StringBuilder(lFormula), 0, lFormula.Length);
            lPEMDAS.ComputeParenthesis();

            // Assert
            Assert.AreEqual(11, lPEMDAS.Chunk.StartIndex);
            Assert.AreEqual(5, lPEMDAS.Chunk.Length);
        }

        #endregion

        #region Tests on exponents

        #region ComputeExponent

        [TestMethod]
        public void ComputeExponent_WithFormula_ReturnsLastChunk()
        {
            PEMDAS lPemdas = new PEMDAS("5+7^(3^(5/2))");

            lPemdas.ComputeExponent();

            Assert.AreEqual(4, lPemdas.Chunk.StartIndex);
            Assert.AreEqual("(3^(5/2))", lPemdas.Chunk.SB.ToString());
            Assert.AreEqual(9, lPemdas.Chunk.Length);
        }

        #endregion

        #region ExtractExponentChunk

        [TestMethod]
        public void ExtractExponentChunk_WithCompleteFormula_ReturnsChunk()
        {
            PEMDAS lPemdas = new PEMDAS("5+7^(3^(5/2))");

            StringBuilder lSb = lPemdas.Chunk.SB;

            lPemdas.ExtractExponentChunk(ref lSb);

            Assert.AreEqual("7^(3^(5/2))", lSb.ToString());
        }

        #endregion

        #region EraseLeftElementsFromExponent

        [TestMethod]
        public void EraseLeftElementsFromExponent_CaseLeftDecimalUnderParenthesis_ReturnsLeftChunk()
        {
            // Arrange
            string lFormula = "(51^(5))";
            int lIndex = 3;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("51^(5))", lSb.ToString());
        }

        [TestMethod]
        public void EraseLeftElementsFromExponent_CaseLeftDecimalWithNoParenthesis_ReturnsLeftChunk()
        {
            // Arrange
            string lFormula = "51^(5)";
            int lIndex = 2;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("51^(5)", lSb.ToString());
        }

        [TestMethod]
        public void EraseLeftElementsFromExponent_CaseLeftDecimalWithOperatorBefore_ReturnsLeftChunk()
        {
            // Arrange
            string lFormula = "5+6^(2)";
            int lIndex = 3;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("6^(2)", lSb.ToString());
        }

        [TestMethod]
        public void EraseLeftElementsFromExponent_CaseLeftDecimalWithMultipleOperatorsBefore_ReturnsLeftChunk()
        {
            // Arrange
            string lFormula = "5*(2+(5^(5/3)))";
            int lIndex = 7;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("5^(5/3)))", lSb.ToString());
        }

        [TestMethod]
        public void EraseLeftElementsFromExponent_CaseLeftDecimalWithOtherExponents_ReturnsLeftChunk()
        {
            // Arrange
            string lFormula = "2^(5^(3/2))";
            int lIndex = 1;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("2^(5^(3/2))", lSb.ToString());
        }

        #endregion

        #region EraseRightElementsFromExponent

        [TestMethod]
        public void EraseRightElementsFromExponent_CaseRightDecimalUnderParenthesis_ReturnsRightChunk()
        {
            // Arrange
            string lFormula = "(51^(5))+5/3";
            int lIndex = 3;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteRightSequenceWithParenthesis(ref lSb, lIndex);

            // Assert
            Assert.AreEqual("(51^(5)", lSb.ToString());
        }

        [TestMethod]
        public void EraseRightElementsFromExponent_CaseRightDecimalWithNoParenthesis_ReturnsRightChunk()
        {
            // Arrange
            string lFormula = "51^(5)";
            int lIndex = 2;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteRightSequenceWithParenthesis(ref lSb, lIndex);

            // Assert
            Assert.AreEqual("51^(5)", lSb.ToString());
        }

        [TestMethod]
        public void EraseRightElementsFromExponent_CaseRightDecimalWithOperatorAfter_ReturnsRightChunk()
        {
            // Arrange
            string lFormula = "5+6^(2)";
            int lIndex = 3;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteRightSequenceWithParenthesis(ref lSb, lIndex);

            // Assert
            Assert.AreEqual("5+6^(2)", lSb.ToString());
        }

        [TestMethod]
        public void EraseRightElementsFromExponent_CaseRightDecimalWithMultipleOperatorsAfter_ReturnsRightChunk()
        {
            // Arrange
            string lFormula = "5*(2+(5^(5/3)))*7";
            int lIndex = 7;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteRightSequenceWithParenthesis(ref lSb, lIndex);

            // Assert
            Assert.AreEqual("5*(2+(5^(5/3)", lSb.ToString());
        }

        [TestMethod]
        public void EraseRightElementsFromExponent_CaseRightDecimalWithOtherExponents_ReturnsRightChunk()
        {
            // Arrange
            string lFormula = "2^(5^(3/2))+5^(2)";
            int lIndex = 1;

            // Act
            StringBuilder lSb = new StringBuilder(lFormula);
            PEMDAS lPEMDAS = new PEMDAS(lFormula);
            lPEMDAS.DeleteRightSequenceWithParenthesis(ref lSb, lIndex);

            // Assert
            Assert.AreEqual("2^(5^(3/2))", lSb.ToString());
        }

        #endregion

        #endregion

        #region Tests on Multiplication or Division

        [TestMethod]
        public void DeleteRightSequence_WithFormula_ReturnsLeftSequence()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+(5*3/2)");

            // Act
            StringBuilder lSb = lPemdas.Chunk.SB;
            lPemdas.DeleteRightSequence(ref lSb, 4);

            // Assert
            Assert.AreEqual("8+(5*3", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void DeleteRightSequence_WithFormula2_ReturnsLeftSequence()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+(5*3.5/2)");

            // Act
            StringBuilder lSb = lPemdas.Chunk.SB;
            lPemdas.DeleteRightSequence(ref lSb, 4);

            // Assert
            Assert.AreEqual("8+(5*3.5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void DeleteLeftSequence_WithFormula_ReturnsLeftSequence()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+(5*3/2)");

            // Act
            StringBuilder lSb = lPemdas.Chunk.SB;
            int lIndex = 4;
            lPemdas.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("5*3/2)", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void DeleteLeftSequence_WithFormula2_ReturnsLeftSequence()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+(5.62*3.5/2)");

            // Act
            StringBuilder lSb = lPemdas.Chunk.SB;
            int lIndex = 4;
            lPemdas.DeleteLeftSequence(ref lSb, ref lIndex);

            // Assert
            Assert.AreEqual("5.62*3.5/2)", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithMultiply_ReturnsMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8*5+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8*5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithMultiply2_ReturnsMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-8*5+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8*5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithDivide_ReturnsDivideChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8/5+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8/5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithDivide2_ReturnsDivideChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-8/5+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8/5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithMultiplyFirstAndDivision_ReturnsMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8*5+3/2+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8*5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithMultiplyFirstAndDivision2_ReturnsMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-8*5+3/2+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8*5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithDivideFirstAndMultiplication_ReturnsDivisionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8/5+3*2+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8/5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithDivideFirstAndMultiplication2_ReturnsDivisionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-8/5+3*2+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("8/5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithMultiplicationFirstAndDivisionAndParenthesis_ReturnsMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-(3*2/5)+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("3*2", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_WithDivisionFirstAndMultiplicationAndParenthesis_ReturnsDivisionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-(3/2*5)+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual("3/2", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_PutDivisionInParenthesis_ReturnsStartIndexAndLengthDivisionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-(3/2*5)+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual(5, lPemdas.Chunk.StartIndex);
            Assert.AreEqual(3, lPemdas.Chunk.Length);
        }

        [TestMethod]
        public void ComputeMultiplicationOrDivision_PutMultiplicationInParenthesis_ReturnsStartIndexAndLengthMultiplicationChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6+2-(3*2/5)+(5^(10))");

            // Act
            lPemdas.ComputeMultiplicationOrDivision();

            // Assert
            Assert.AreEqual(5, lPemdas.Chunk.StartIndex);
            Assert.AreEqual(3, lPemdas.Chunk.Length);
        }

        #endregion

        #region Tests on Addition or Substraction  

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithAddition_ReturnsAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+5*(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8+5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithAddition2_ReturnsAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/8+5+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8+5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithSubstraction_ReturnsSubstractChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8-5+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8-5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithSubstract2_ReturnsSubstractChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/8-5+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8-5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithAdditionFirstAndSubstraction_ReturnsAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8+5-3/2+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8+5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithAdditionAndSubstraction2_ReturnsAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/8+5-3/2+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8+5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithSubstractionFirstAndAddition_ReturnsSubstractionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("8-5*3+2*(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("8-5", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithSubstractionFirstAndAddition2_ReturnsSubstractionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2-8+5+3*2+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("2-8", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithAdditionFirstAndSubstractionAndParenthesis_ReturnsAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/(3+2-5)+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("3+2", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_WithSubstractionFirstAndAdditionAndParenthesis_ReturnsSubstractionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/(3-2+5)+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual("3-2", lPemdas.Chunk.SB.ToString());
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_PutSubstractionInParenthesis_ReturnsStartIndexAndLengthSubstractionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/(3-2+5)+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual(5, lPemdas.Chunk.StartIndex);
            Assert.AreEqual(3, lPemdas.Chunk.Length);
        }

        [TestMethod]
        public void ComputeAdditionOrSubstraction_PutAdditionInParenthesis_ReturnsStartIndexAndLengthaAdditionChunk()
        {
            // Arrange
            PEMDAS lPemdas = new PEMDAS("6*2/(3+2-5)+(5^(10))");

            // Act
            lPemdas.ComputeAdditionOrSubstraction();

            // Assert
            Assert.AreEqual(5, lPemdas.Chunk.StartIndex);
            Assert.AreEqual(3, lPemdas.Chunk.Length);
        }

        #endregion

        #region Test ExtractOperands

        [TestMethod]
        public void ExtractOperands_SetSimpleMultiplicationFormula_ReturnsOperands()
        {
            PEMDAS lPemdas = new PEMDAS("(40.56*15)");

            lPemdas.ExtractOperands(out decimal a, out decimal b);

            Assert.AreEqual(40.56m, a);
            Assert.AreEqual(15, b);
        }

        [TestMethod]
        public void ExtractOperands_SetSimpleMultiplicationFormula2_ReturnsOperands()
        {
            PEMDAS lPemdas = new PEMDAS("(40,56*15)");

            lPemdas.ExtractOperands(out decimal a, out decimal b);

            Assert.AreEqual(40.56m, a);
            Assert.AreEqual(15, b);
        }

        [TestMethod]
        public void ExtractOperands_SetSimpleDivisionFormula_ReturnsOperands()
        {
            PEMDAS lPemdas = new PEMDAS("(40/15)");

            lPemdas.ExtractOperands(out decimal a, out decimal b);

            Assert.AreEqual(40, a);
            Assert.AreEqual(15, b);
        }

        #endregion

        #region  GetNumberFromChunk

        [TestMethod]
        public void GetNumberFromChunk_SetSimpleFormula_ReturnsLeftOperand()
        {
            PEMDAS lPemdas = new PEMDAS("16.5+56");

            string lChunk = string.Empty;

            lPemdas.GetNumberFromChunk(ref lChunk);

            Assert.AreEqual("16.5", lChunk);
        }

        [TestMethod]
        public void GetNumberFromChunk_SetSimpleFormula2_ReturnsRightOperand()
        {
            PEMDAS lPemdas = new PEMDAS("16.5+56");

            string lChunk = string.Empty;

            lPemdas.GetNumberFromChunk(ref lChunk, 5);

            Assert.AreEqual("56", lChunk);
        }

        #endregion

        #region  GetIndexOperator

        [TestMethod]
        public void GetIndexOperator_WithAddition_ReturnsIndex()
        {
            PEMDAS lPemdas = new PEMDAS("16.5+56");

            string lChunk = string.Empty;

            int lIndex = lPemdas.GetIndexOperator();

            Assert.AreEqual(4, lIndex);
        }

        [TestMethod]
        public void GetIndexOperator_WithSubstraction_ReturnsIndex()
        {
            PEMDAS lPemdas = new PEMDAS("16.5-56");

            string lChunk = string.Empty;

            int lIndex = lPemdas.GetIndexOperator();

            Assert.AreEqual(4, lIndex);
        }

        [TestMethod]
        public void GetIndexOperator_WithMultiplication_ReturnsIndex()
        {
            PEMDAS lPemdas = new PEMDAS("16.5*56");

            string lChunk = string.Empty;

            int lIndex = lPemdas.GetIndexOperator();

            Assert.AreEqual(4, lIndex);
        }

        [TestMethod]
        public void GetIndexOperator_WitDivision_ReturnsIndex()
        {
            PEMDAS lPemdas = new PEMDAS("16.5/56");

            string lChunk = string.Empty;

            int lIndex = lPemdas.GetIndexOperator();

            Assert.AreEqual(4, lIndex);
        }

        #endregion

        #region DoCompute

        #region Addition

        [TestMethod]
        public void DoCompute_WithAddition_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15+16");
            lPemdas.Operand = Operand.Addition;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("31", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithAddition2_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15.67+16.20");
            lPemdas.Operand = Operand.Addition;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("31,87", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithAddition3_ReturnsException()
        {
            try
            {
                PEMDAS lPemdas = new PEMDAS("0+999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
                lPemdas.Operand = Operand.Addition;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("La valeur était trop grande ou trop petite pour un Decimal.", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        #endregion

        #region Substraction

        [TestMethod]
        public void DoCompute_WithSubstraction_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15-16");
            lPemdas.Operand = Operand.Substraction;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("-1", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithSubstraction2_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15.67-16.20");
            lPemdas.Operand = Operand.Substraction;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("-0,53", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithSubstraction3_ReturnsException()
        {
            try
            {
                PEMDAS lPemdas = new PEMDAS("0-999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999999");
                lPemdas.Operand = Operand.Substraction;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("La valeur était trop grande ou trop petite pour un Decimal.", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        #endregion

        #region Multiplication

        [TestMethod]
        public void DoCompute_WithMultiplication_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15*16");
            lPemdas.Operand = Operand.Multiplication;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("240", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithMultiplication2_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15.67*16.20");
            lPemdas.Operand = Operand.Multiplication;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("253,8540", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithMultiplication3_ReturnsException()
        {
            try
            {
                PEMDAS lPemdas = new PEMDAS("156700000000000000000000000000000000000000000000000000000000000000*1600000000000000000000000000000000000000000000000000000000000000020");
                lPemdas.Operand = Operand.Multiplication;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("La valeur était trop grande ou trop petite pour un Decimal.", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        #endregion

        #region Division

        [TestMethod]
        public void DoCompute_WithDivision_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15/16");
            lPemdas.Operand = Operand.Division;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("0,9375", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithDivision2_ReturnsException()
        {
            PEMDAS lPemdas = new PEMDAS("15.67/16.20");
            lPemdas.Operand = Operand.Division;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("0,9672839506172839506172839506", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithDivision3_ReturnsException()
        {
            try
            {
                PEMDAS lPemdas = new PEMDAS("15.67/0");
                lPemdas.Operand = Operand.Division;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("Tentative de division par zéro.", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        [TestMethod]
        public void DoCompute_WithDivision4_ReturnsException()
        {
            PEMDAS lPemdas = new PEMDAS("1/0.99999999999999999999999999999999999999999999999999999999999999999999999999999");
            lPemdas.Operand = Operand.Division;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("1,000000000000000000000000001", lResult.ToString());
        }

        #endregion

        #region Exponent

        [TestMethod]
        public void DoCompute_WithExponent_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15^2");
            lPemdas.Operand = Operand.Exponent;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("225", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithExponent2_ReturnsResult()
        {
            PEMDAS lPemdas = new PEMDAS("15^16");
            lPemdas.Operand = Operand.Exponent;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("6568408355712890000", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithExponent3_ReturnsException()
        {
            try
            {
                PEMDAS lPemdas = new PEMDAS("150^1000");
                lPemdas.Operand = Operand.Exponent;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("La valeur était trop grande ou trop petite pour un Decimal.", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        #endregion

        #region Square

        [TestMethod]
        public void DoCompute_WithRootSquare_ReturnsResult()
        {
            Assert.Inconclusive();
            PEMDAS lPemdas = new PEMDAS("√81");
            lPemdas.Operand = Operand.Square;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("225", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithRootSquare2_ReturnsResult()
        {
            Assert.Inconclusive();
            PEMDAS lPemdas = new PEMDAS("√15");
            lPemdas.Operand = Operand.Square;

            lPemdas.DoCompute(out decimal lResult);

            Assert.AreEqual("6568408355712890000", lResult.ToString());
        }

        [TestMethod]
        public void DoCompute_WithRootSquare3_ReturnsException()
        {
            Assert.Inconclusive();
            try
            {
                PEMDAS lPemdas = new PEMDAS("√-1");
                lPemdas.Operand = Operand.Square;

                lPemdas.DoCompute(out decimal lResult);
            }
            catch (Exception e)
            {
                Assert.AreEqual("TODO", e.Message);
                return;
            }

            Assert.Fail("Exception was raised but not catched");
        }

        #endregion

        #endregion

    }
    }
