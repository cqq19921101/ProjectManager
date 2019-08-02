$(function () {
    //http://stackoverflow.com/questions/3514784/what-is-the-best-way-to-detect-a-handheld-device-in-jquery
    if(/android|webos|iphone|ipad|ipod|blackberry/i.test(navigator.userAgent.toLowerCase()))
        $('#Portal-Main-Menu').dcMegaMenu({
            rowItems: '4',
            speed: 0,
            effect: 'fade',
            event: 'click'
        });
    else
        $('#Portal-Main-Menu').dcMegaMenu({
            rowItems: '4',
            speed: 0
        });
});