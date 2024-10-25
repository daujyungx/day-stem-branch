<Query Kind = "Statements" />

var now = DateTime.UtcNow;

// 1949-10-01 是甲子日
var d0 = new DateTime(1949, 10, 1);

var 天干 = "甲乙丙丁戊己庚辛壬癸";
var 地支 = "子丑寅卯辰巳午未申酉戌亥";
var 干支 = Enumerable.Range(0, 60).Select(i => new
{
    v = $"{天干[i % 10]}{地支[i % 12]}",
    d = d0.AddDays(i),
    i = i,
})
//.Dump()
.ToList();

$"""
BEGIN:VCALENDAR
METHOD:PUBLISH
PRODID:daujyungx
VERSION:2.0
CALSCALE:GREGORIAN
X-WR-CALNAME:日干支
X-WR-TIMEZONE:Asia/Shanghai
BEGIN:VTIMEZONE
TZID:China Standard Time
BEGIN:STANDARD
DTSTART:16010101T000000
TZOFFSETFROM:+0800
TZOFFSETTO:+0800
END:STANDARD
BEGIN:DAYLIGHT
DTSTART:16010101T000000
TZOFFSETFROM:+0800
TZOFFSETTO:+0800
END:DAYLIGHT
END:VTIMEZONE
{string.Join("\n", 干支.Select(x =>
$"""
BEGIN:VEVENT
DTSTART;VALUE=DATE:{x.d:yyyyMMdd}
DTEND;VALUE=DATE:{x.d.AddDays(1):yyyyMMdd}
RRULE:FREQ=DAILY;INTERVAL=60
DTSTAMP:{now:yyyyMMdd'T'HHmmss'Z'}
UID:{Guid.NewGuid().ToString("n")}
CLASS:PUBLIC
SEQUENCE:0
STATUS:CONFIRMED
SUMMARY:{x.v}日
TRANSP:TRANSPARENT
X-MICROSOFT-CDO-ALLDAYEVENT:TRUE
END:VEVENT
"""))}
END:VCALENDAR

""".Dump("ics");

