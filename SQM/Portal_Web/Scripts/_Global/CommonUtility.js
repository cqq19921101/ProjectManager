//Return YYYY/MM/DD
function GetCurrentDate()
{
    var date = new Date();
    var curr_year = date.getFullYear();
    var curr_month = date.getMonth() + 1;
    var curr_date = date.getDate();

    return curr_year + "/" + curr_month + "/" + curr_date;
}

function SetCurrentDate(m,d) {
    var date = new Date();
    var curr_year = date.getFullYear();
    var curr_month = date.getMonth() + 1 + m;
    var curr_date = date.getDate() + d;

    return curr_year + "/" + curr_month + "/" + curr_date;
}

/* 左邊補0 */
function padLeft(str, len) {
    str = '' + str;
    return str.length >= len ? str : new Array(len - str.length + 1).join("0") + str;
}

/* 右邊補0 */
function padRight(str, len) {
    str = '' + str;
    return str.length >= len ? str : str + new Array(len - str.length + 1).join("0");
}

/* JS : 小數點第N位四捨五入 */
/* formatFloat("1109.1893", 2) = 1109.19*/
function formatFloat(num, pos) {
    var size = Math.pow(10, pos);
    return Math.round(num * size) / size;
}