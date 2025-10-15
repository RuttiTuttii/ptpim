using TransactionLib;

public class TransactionAnalyzerTests
{
    [Fact]
    public void RejectsUnknownTransactionType()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Unknown,
            Timestamp = DateTime.Now
        };

        Assert.StartsWith("������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsTooManyTransactionsForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Deposit,
            DailyTransactionCount = 11,
            IsVipClient = false
        };

        Assert.Contains("��������� ����������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsExactly10TransactionsForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Deposit,
            DailyTransactionCount = 10,
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsManyTransactionsForVip()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Deposit,
            DailyTransactionCount = 15,
            IsVipClient = true
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsVipToTransferLargeExternal()
    {
        var tx = new Transaction
        {
            Amount = 2_000_000,
            Kind = TransactionKind.Transfer,
            IsInternal = false,
            IsVipClient = true,
            DailyTransactionTotal = 0 // �������� �������� �������� ������
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsLargeExternalTransferForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 1_000_001,
            Kind = TransactionKind.Transfer,
            IsInternal = false,
            IsVipClient = false,
            DailyTransactionTotal = -1_000_001 // �������� �������� �������� ������
        };

        Assert.StartsWith("���������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsExternalTransferLimitForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 1_000_000,
            Kind = TransactionKind.Transfer,
            IsInternal = false,
            IsVipClient = false,
            DailyTransactionTotal = -1_000_000
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AppliesCommissionForNonVipCurrentToSaving()
    {
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            IsInternal = false,
            IsVipClient = false
        };

        Assert.StartsWith("��������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void NoCommissionForSameAccountType()
    {
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "�������",
            IsInternal = false,
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void NoCommissionForVipDifferentAccountTypes()
    {
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            IsInternal = false,
            IsVipClient = true
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void NoCommissionForInternalTransfers()
    {
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            IsInternal = true,
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsDailySumExceededForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 250_001,
            Kind = TransactionKind.Transfer,
            DailyTransactionTotal = 250_000,
            IsVipClient = false
        };

        Assert.StartsWith("�����������: ���������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsDailySumLimitForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 250_000,
            Kind = TransactionKind.Deposit,
            DailyTransactionTotal = 250_000,
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsUnlimitedDailySumForVip()
    {
        var tx = new Transaction
        {
            Amount = 1_000_000,
            Kind = TransactionKind.Transfer,
            DailyTransactionTotal = 500_000,
            IsVipClient = true
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsLargeAtmTransfer()
    {
        var tx = new Transaction
        {
            Amount = 100_001,
            Kind = TransactionKind.Transfer,
            Channel = "ATM",
            IsInternal = true
        };

        Assert.StartsWith("�����������: ���������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsAtmLimitTransfer()
    {
        var tx = new Transaction
        {
            Amount = 100_000,
            Kind = TransactionKind.Transfer,
            Channel = "ATM",
            IsInternal = true
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsVipLargeExternalWithdrawal()
    {
        var tx = new Transaction
        {
            Amount = 500_000,
            Kind = TransactionKind.Withdrawal,
            IsInternal = false,
            IsVipClient = true,
            Channel = "Office"
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsLargeExternalWithdrawalForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 200_001,
            Kind = TransactionKind.Withdrawal,
            IsInternal = false,
            IsVipClient = false,
            Channel = "Office"
        };

        Assert.StartsWith("�����������: ������� ������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsExternalWithdrawalLimitForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 200_000,
            Kind = TransactionKind.Withdrawal,
            IsInternal = false,
            IsVipClient = false,
            Channel = "Office"
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsZeroAmount()
    {
        var tx = new Transaction
        {
            Amount = 0,
            Kind = TransactionKind.Unknown,
            IsVipClient = false,
            Channel = "Web",
            Timestamp = DateTime.Now
        };

        Assert.StartsWith("������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsNegativeAmount()
    {
        var tx = new Transaction
        {
            Amount = -100,
            Kind = TransactionKind.Deposit,
            IsVipClient = false,
            Channel = "Web",
            Timestamp = DateTime.Now
        };

        Assert.StartsWith("������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsWebWithdrawal()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Withdrawal,
            Channel = "Web",
            IsVipClient = false
        };

        Assert.StartsWith("���������: ������ ����� ���-������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsMobileWithdrawal()
    {
        var tx = new Transaction
        {
            Amount = 100,
            Kind = TransactionKind.Withdrawal,
            Channel = "Mobile",
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsWeekendOnlineDifferentAccountTypeTransferMobile()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Mobile",
            Timestamp = weekend,
            IsVipClient = true, // VIP ����� �������� ��������
            IsInternal = true // ���������� ����� �������� ��������
        };

        Assert.StartsWith("������: � �������� ������ ���������� ����� ������� ������ ������ ����� ������-������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsWeekendOnlineDifferentAccountTypeTransferWeb()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Web",
            Timestamp = weekend,
            IsVipClient = true, // VIP ����� �������� ��������
            IsInternal = true // ���������� ����� �������� ��������
        };

        Assert.StartsWith("������: � �������� ������ ���������� ����� ������� ������ ������ ����� ������-������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void RejectsWeekendOnlineDifferentAccountTransfer()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Web",
            Timestamp = weekend,
            IsVipClient = true, // VIP ����� �������� ��������
            IsInternal = true // ���������� ����� �������� ��������
        };

        Assert.StartsWith("������: � �������� ������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsWeekendOnlineSameAccountTransfer()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "�������",
            Channel = "Web",
            Timestamp = weekend,
            IsVipClient = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AppliesCommissionForWeekendOfficeDifferentAccountTransfer()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Office",
            Timestamp = weekend,
            IsVipClient = false,
            IsInternal = false
        };

        Assert.StartsWith("��������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AppliesCommissionForWeekdayOnlineDifferentAccountTransfer()
    {
        var weekday = new DateTime(2024, 1, 3); // �����
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Web",
            Timestamp = weekday,
            IsVipClient = false,
            IsInternal = false
        };

        Assert.StartsWith("��������", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsInternalLargeTransferForNonVip()
    {
        var tx = new Transaction
        {
            Amount = 2_000_000,
            Kind = TransactionKind.Transfer,
            IsInternal = true,
            IsVipClient = false,
            DailyTransactionTotal = -2_000_000 // �������� �������� �������� ������
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsWeekendOfficeDifferentAccountInternalTransfer()
    {
        var weekend = new DateTime(2024, 1, 6); // �������
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Office",
            Timestamp = weekend,
            IsVipClient = false,
            IsInternal = true // ���������� ���������� - ��� ��������
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void AllowsWeekdayOnlineDifferentAccountVipTransfer()
    {
        var weekday = new DateTime(2024, 1, 3); // �����
        var tx = new Transaction
        {
            Amount = 1000,
            Kind = TransactionKind.Transfer,
            FromAccountType = "�������",
            ToAccountType = "��������������",
            Channel = "Web",
            Timestamp = weekday,
            IsVipClient = true, // VIP - ��� ��������
            IsInternal = false
        };

        Assert.Equal("���������� ���������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }

    [Fact]
    public void NullableAmount()
    {
        var tx = new Transaction
        {
            Amount = 0
        };
        Assert.StartsWith("������: ����� ������ ���� �������������.", TransactionAnalyzer.AnalyzeTransaction(tx));
    }
}