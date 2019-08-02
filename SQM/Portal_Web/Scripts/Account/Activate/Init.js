(function ($) {
    //處理setfocus在某些環境不work的狀況
    jQuery.fn.setfocus = function () {
        return this.each(function () {
            var dom = this;
            setTimeout(function () {
                try { dom.focus(); } catch (e) { }
            }, 0);
        });
    };
})(jQuery);

$(function () {
    $('#pwdPWD1').setfocus();
});