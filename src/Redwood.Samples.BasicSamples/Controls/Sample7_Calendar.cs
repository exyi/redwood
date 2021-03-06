﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Redwood.Framework.Binding;
using Redwood.Framework.Controls;
using Redwood.Framework.Hosting;

namespace Redwood.Samples.BasicSamples.Controls
{
    public class Sample7_Calendar : RedwoodMarkupControl 
    {

        public DateTime? SelectedDate
        {
            get { return (DateTime?)GetValue(SelectedDateProperty); }
            set { SetValue(SelectedDateProperty, value); }
        }
        public static RedwoodProperty SelectedDateProperty = RedwoodProperty.RegisterControlStateProperty<DateTime?, Sample7_Calendar>(c => c.SelectedDate);


        public DateTime VisibleDate 
        {
            get { return (DateTime)GetValue(VisibleDateProperty); }
            set { SetValue(VisibleDateProperty, value); }
        }
        public static RedwoodProperty VisibleDateProperty = RedwoodProperty.RegisterControlStateProperty<DateTime, Sample7_Calendar>(c => c.VisibleDate, DateTime.Now);


        public override bool RequiresControlState
        {
            get { return true; }
        }
         


        public string[] DayNames
        {
            get { return CultureInfo.CurrentUICulture.DateTimeFormat.AbbreviatedDayNames; }
        }
        public static RedwoodProperty DayNamesProperty = RedwoodProperty.RegisterControlStateProperty<string[], Sample7_Calendar>(c => c.DayNames);


        public string VisibleMonthText
        {
            get { return VisibleDate.ToString("MMMM yyyy"); }
        }
        public static RedwoodProperty VisibleMonthTextProperty = RedwoodProperty.RegisterControlStateProperty<string, Sample7_Calendar>(c => c.VisibleMonthText);



        public IList<CalendarRow> Rows
        {
            get { return GetRows().ToList(); }  
        }
        public static RedwoodProperty RowsProperty = RedwoodProperty.RegisterControlStateProperty<IEnumerable<CalendarRow>, Sample7_Calendar>(c => c.Rows);

        private IEnumerable<CalendarRow> GetRows()
        {
            var firstMonthDate = new DateTime(VisibleDate.Year, VisibleDate.Month, 1);
            var date = firstMonthDate.AddDays(-(int)firstMonthDate.DayOfWeek);

            while (date < firstMonthDate.AddMonths(1))
            {
                yield return new CalendarRow()
                {
                    Days = Enumerable.Range(0, 7).Select(i => date.AddDays(i)).Select(d => new CalendarDay()
                    {
                        IsOtherMonth = d.Month != VisibleDate.Month,
                        IsSelected = SelectedDate != null && d == SelectedDate.Value.Date,
                        IsToday = d == DateTime.Today,
                        Number = d.Day,
                        Date = d
                    }).ToArray()
                };
                date = date.AddDays(7);
            }
        } 


        public void GoToPreviousMonth()
        {
            VisibleDate = VisibleDate.AddMonths(-1);
        }

        public void GoToNextMonth()
        {
            VisibleDate = VisibleDate.AddMonths(1);
        }

        public void SelectDate(DateTime? date)
        {
            SelectedDate = date;
        }

    }

    public class CalendarRow
    {
        public CalendarDay[] Days { get; set; }
    }

    public class CalendarDay
    {
        public int Number { get; set; }

        public bool IsOtherMonth { get; set; }

        public bool IsSelected { get; set; }

        public bool IsToday { get; set; }
    
        public  DateTime Date { get; set; }
    }
}