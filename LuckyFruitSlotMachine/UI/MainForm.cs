using System.Drawing;
using FruitSlot.Data;
using FruitSlot.Engine;
using FruitSlot.Models;
using FruitSlot.Reports;

namespace FruitSlot.UI;

public class MainForm : Form
{
    private readonly ReelStrip[] _reels      = ReelDefinitions.GetReels();
    private readonly PayTable    _payTable   = new();
    private readonly SpinEngine  _spinEngine;
    private SpinResult?          _lastResult;
    private int                  _balance    = 100;

    private bool _isSpinning = false;
    private int  _spinTick   = 0;
    private const int LockReel1 = 14, LockReel2 = 19, LockReel3 = 24;
    private readonly Random _animRng = new();

    private readonly ReelPanel[] _reelPanels = new ReelPanel[3];
    private Label  _lblWin     = null!;
    private Label  _lblBalance = null!;
    private Button _btnSpin    = null!;
    private readonly System.Windows.Forms.Timer _animTimer = new() { Interval = 60 };

    public MainForm()
    {
        _spinEngine = new SpinEngine(_reels, _payTable);
        BuildUI();
        _animTimer.Tick += OnAnimTick;
    }

    private void BuildUI()
    {
        Text            = "Lucky Fruits";
        Size            = new Size(500, 540);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox     = false;
        BackColor       = Color.FromArgb(28, 12, 60);
        DoubleBuffered  = true;
        StartPosition   = FormStartPosition.CenterScreen;

        Controls.Add(MakeLabel("LUCKY FRUITS", new Font("Segoe UI", 22f, FontStyle.Bold),
            Color.Gold, new Rectangle(0, 18, 494, 52)));

        int[] reelX = { 38, 178, 318 };
        for (int i = 0; i < 3; i++)
        {
            _reelPanels[i] = new ReelPanel { Bounds = new Rectangle(reelX[i], 85, 136, 150) };
            Controls.Add(_reelPanels[i]);
        }

        Controls.Add(MakeLabel("Cherry ×10  |  Lemon ×15  |  Orange ×20  |  Plum ×30  |  Bar ×100  |  7 ×500",
            new Font("Segoe UI", 8f), Color.FromArgb(180, 160, 220), new Rectangle(0, 248, 494, 22)));

        _lblWin = MakeLabel("Press SPIN to play", new Font("Segoe UI", 14f, FontStyle.Bold),
            Color.Gold, new Rectangle(0, 278, 494, 40));
        Controls.Add(_lblWin);

        _lblBalance = MakeLabel($"Balance: {_balance} credits", new Font("Segoe UI", 10f),
            Color.LightGray, new Rectangle(0, 320, 494, 28));
        Controls.Add(_lblBalance);

        _btnSpin = MakeButton("SPIN", new Font("Segoe UI", 18f, FontStyle.Bold),
            Color.White, Color.Crimson, new Rectangle(160, 358, 174, 58));
        _btnSpin.Click += OnSpinClick;
        Controls.Add(_btnSpin);

        var btnPar = MakeButton("Export Par Sheet (CSV)", new Font("Segoe UI", 9f),
            Color.Gold, Color.FromArgb(55, 35, 100), new Rectangle(155, 432, 184, 30));
        btnPar.FlatAppearance.BorderColor = Color.FromArgb(120, 100, 60);
        btnPar.FlatAppearance.BorderSize  = 1;
        btnPar.Click += OnExportParSheet;
        Controls.Add(btnPar);
    }

    private static Label MakeLabel(string text, Font font, Color color, Rectangle bounds) => new()
    {
        Text      = text,
        Font      = font,
        ForeColor = color,
        AutoSize  = false,
        TextAlign = ContentAlignment.MiddleCenter,
        BackColor = Color.Transparent,
        Bounds    = bounds
    };

    private static Button MakeButton(string text, Font font, Color fore, Color back, Rectangle bounds)
    {
        var btn = new Button
        {
            Text      = text,
            Font      = font,
            ForeColor = fore,
            BackColor = back,
            FlatStyle = FlatStyle.Flat,
            Bounds    = bounds,
            Cursor    = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
    }

    private void OnSpinClick(object? sender, EventArgs e)
    {
        if (_isSpinning || _balance < 1) return;
        _balance--;
        _isSpinning       = true;
        _spinTick         = 0;
        _lastResult       = _spinEngine.Spin();
        _btnSpin.Enabled  = false;
        _lblWin.Text      = "Spinning...";
        _lblWin.ForeColor = Color.Gold;
        _lblBalance.Text  = $"Balance: {_balance} credits";
        _animTimer.Start();
    }

    private void OnAnimTick(object? sender, EventArgs e)
    {
        _spinTick++;
        int[] lockAt = { LockReel1, LockReel2, LockReel3 };

        for (int i = 0; i < 3; i++)
        {
            _reelPanels[i].DisplaySymbol = _spinTick >= lockAt[i]
                ? _lastResult!.Symbols[i]
                : (Symbol)_animRng.Next(6);
            _reelPanels[i].Invalidate();
        }

        if (_spinTick < LockReel3) return;

        _animTimer.Stop();
        _isSpinning = false;

        if (_lastResult!.IsWin)
        {
            _balance         += _lastResult.Prize;
            _lblWin.Text      = $"WIN!  +{_lastResult.Prize} credits";
            _lblWin.ForeColor  = Color.LightGreen;
        }
        else
        {
            _lblWin.Text      = "No win — try again!";
            _lblWin.ForeColor  = Color.FromArgb(180, 160, 210);
        }

        _lblBalance.Text  = $"Balance: {_balance} credits";
        _btnSpin.Enabled  = _balance >= 1;
    }

    private void OnExportParSheet(object? sender, EventArgs e)
    {
        try
        {
            using var dlg = new SaveFileDialog
            {
                Title            = "Save Par Sheet",
                Filter           = "CSV files (*.csv)|*.csv",
                FileName         = "SlotParSheet.csv",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop)
            };

            if (dlg.ShowDialog(this) != DialogResult.OK) return;

            ParSheetGenerator.Write(dlg.FileName, _reels, _payTable);
            MessageBox.Show($"Par sheet saved to:\n{dlg.FileName}", "Done",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Export failed:\n{ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
