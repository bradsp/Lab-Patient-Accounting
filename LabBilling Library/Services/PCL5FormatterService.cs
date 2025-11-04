using System;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for generating PCL5 (Printer Control Language) escape sequences.
/// Optimized for dot-matrix printers with pin-fed forms.
/// </summary>
public class PCL5FormatterService
{
    // PCL5 Escape sequences
    private const char ESC = '\x1B';  // Escape character
    private const string RESET = "\x1B" + "E";  // Reset printer
    private const string FORM_FEED = "\f";  // Form feed (eject page)
    private const string LINE_FEED = "\n";  // Line feed
    private const string CARRIAGE_RETURN = "\r";  // Carriage return

    /// <summary>
    /// Initializes the printer with standard settings for forms printing.
    /// </summary>
    /// <returns>PCL5 initialization string</returns>
    public string InitializePrinter()
    {
        StringBuilder sb = new StringBuilder();
        
     // Reset printer to default state
    sb.Append(RESET);
      
    // Set to 10 pitch (10 characters per inch) - standard for forms
      sb.Append($"{ESC}(s10H");
        
// Set line spacing to 6 lines per inch (standard)
        sb.Append($"{ESC}&l6D");
      
        // Disable auto line feed
        sb.Append($"{ESC}&k0G");
        
        // Set top margin to 0 (for pre-printed forms)
        sb.Append($"{ESC}&l0E");
        
        return sb.ToString();
    }

    /// <summary>
    /// Sets the horizontal position (column) on the page.
    /// </summary>
    /// <param name="column">Column number (0-based, 10 columns per inch at 10 pitch)</param>
    /// <returns>PCL5 command string</returns>
    public string SetHorizontalPosition(int column)
    {
        // PCL5 uses decipoints (1/720 inch)
        // At 10 pitch, each character is 0.1 inch = 72 decipoints
        int decipoints = column * 72;
        return $"{ESC}*p{decipoints}X";
    }

    /// <summary>
    /// Sets the vertical position (row/line) on the page.
    /// </summary>
    /// <param name="row">Row number (0-based, 6 lines per inch at standard spacing)</param>
    /// <returns>PCL5 command string</returns>
    public string SetVerticalPosition(int row)
    {
        // At 6 lines per inch, each line is 0.1667 inch = 120 decipoints
        int decipoints = row * 120;
      return $"{ESC}*p{decipoints}Y";
    }

    /// <summary>
/// Sets absolute position on the page.
    /// </summary>
    /// <param name="row">Row number (lines from top)</param>
    /// <param name="column">Column number (characters from left)</param>
    /// <returns>PCL5 command string</returns>
    public string SetPosition(int row, int column)
{
        return SetVerticalPosition(row) + SetHorizontalPosition(column);
    }

    /// <summary>
    /// Moves to the specified line (relative positioning).
    /// </summary>
    /// <param name="lines">Number of lines to move (positive for down, negative for up)</param>
 /// <returns>PCL5 command string</returns>
    public string MoveVertical(int lines)
    {
     if (lines > 0)
        {
    // Move down
        return $"{ESC}&a+{lines}R";
        }
     else if (lines < 0)
      {
            // Move up
            return $"{ESC}&a{lines}R";
        }
        return string.Empty;
    }

    /// <summary>
    /// Moves to the specified column (relative positioning).
    /// </summary>
    /// <param name="columns">Number of columns to move (positive for right, negative for left)</param>
    /// <returns>PCL5 command string</returns>
    public string MoveHorizontal(int columns)
    {
        if (columns > 0)
        {
        return $"{ESC}&a+{columns}C";
   }
        else if (columns < 0)
        {
            return $"{ESC}&a{columns}C";
        }
        return string.Empty;
 }

    /// <summary>
    /// Sets the font to Courier (fixed-width, ideal for forms).
    /// </summary>
    /// <param name="pitch">Characters per inch (typically 10 or 12)</param>
    /// <returns>PCL5 command string</returns>
    public string SetCourierFont(int pitch = 10)
    {
      StringBuilder sb = new StringBuilder();
        
    // Select Courier font
    sb.Append($"{ESC}(s0p{pitch}h0s0b3T");
    
        return sb.ToString();
    }

    /// <summary>
    /// Sets bold text.
    /// </summary>
    /// <param name="enabled">True to enable bold, false to disable</param>
    /// <returns>PCL5 command string</returns>
    public string SetBold(bool enabled)
    {
      return enabled ? $"{ESC}(s3B" : $"{ESC}(s0B";
    }

    /// <summary>
    /// Ejects the current page (form feed).
    /// </summary>
    /// <returns>Form feed character</returns>
    public string EjectPage()
    {
        return FORM_FEED;
    }

    /// <summary>
    /// Advances to the next line.
 /// </summary>
    /// <returns>Carriage return + line feed</returns>
    public string NextLine()
    {
        return CARRIAGE_RETURN + LINE_FEED;
    }

    /// <summary>
    /// Returns to the beginning of the current line.
    /// </summary>
    /// <returns>Carriage return</returns>
    public string CarriageReturn()
  {
        return CARRIAGE_RETURN;
    }

    /// <summary>
    /// Advances one line without carriage return.
    /// </summary>
    /// <returns>Line feed</returns>
    public string LineFeed()
  {
     return LINE_FEED;
    }

    /// <summary>
    /// Creates spacing using spaces (for simple horizontal positioning).
    /// More reliable than tab characters on dot-matrix printers.
    /// </summary>
  /// <param name="count">Number of spaces</param>
    /// <returns>String of spaces</returns>
    public string Spaces(int count)
    {
        return new string(' ', Math.Max(0, count));
    }

    /// <summary>
    /// Formats a text line with left margin spacing.
  /// </summary>
    /// <param name="text">Text to print</param>
    /// <param name="leftMargin">Number of characters to indent from left edge</param>
    /// <returns>Formatted string with spacing and newline</returns>
    public string FormatLine(string text, int leftMargin)
    {
    return Spaces(leftMargin) + text + NextLine();
    }

    /// <summary>
    /// Sets page length for forms (prevents automatic page breaks).
    /// </summary>
    /// <param name="lines">Number of lines per page (typically 66 for 11" forms at 6 LPI)</param>
    /// <returns>PCL5 command string</returns>
    public string SetPageLength(int lines)
    {
        return $"{ESC}&l{lines}F";
    }

    /// <summary>
    /// Disables automatic perforation skip (important for pin-fed forms).
    /// </summary>
    /// <returns>PCL5 command string</returns>
    public string DisablePerforationSkip()
    {
        return $"{ESC}&l0L";
    }

    /// <summary>
    /// Complete initialization for pin-fed forms printing.
    /// </summary>
    /// <returns>Full initialization string</returns>
    public string InitializeForPinFedForms()
    {
        StringBuilder sb = new StringBuilder();
  
        // Reset and basic setup
        sb.Append(InitializePrinter());
        
        // Disable perforation skip (critical for pin-fed forms)
        sb.Append(DisablePerforationSkip());
        
   // Set page length (11" at 6 LPI = 66 lines)
        sb.Append(SetPageLength(66));
        
        // Set Courier font at 10 pitch
    sb.Append(SetCourierFont(10));
        
        return sb.ToString();
    }

    /// <summary>
    /// Resets printer to default state.
    /// </summary>
  /// <returns>PCL5 reset command</returns>
    public string ResetPrinter()
    {
        return RESET;
    }
}
