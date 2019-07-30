 var Print = function(value) {
    var leftvalue = screen.availWidth - 10;
    var topvalue = screen.availHeight - 30;
    var sValue = 'width=400,height=200,top=' + (topvalue - 100) * 0.5 + ',left=' + (leftvalue - 300) * 0.5;
    var NewUrl = value;
    return window.open(value, '', sValue)
    };