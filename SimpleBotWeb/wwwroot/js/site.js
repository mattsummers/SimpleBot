(function ($) {

    $.fn.passwordStrength = function (options) {

        var postUrl = "/Account/GetPasswordStrength";
        var bad = "#FF0000";
        var weak = "#ff6600";
        var medium = "#cc9900";
        var strong = "#cccc00";
        var verystrong = "#006600";

        var settings = $.extend({
            input: ""
        }, options);

        return this.each(function () {

            var targetElement = $(this);
            var timer;
            $(options.input).on("change keyup", function () {
                window.clearTimeout(timer);
                timer = window.setTimeout(calculate, 500);
            });

            calculate();

            function calculate() {
                var passwordString = $(options.input).val();
                var dataObject = { password: passwordString };

                // Get password strength
                $.post(postUrl, dataObject).done(function (data) {
                    var scoreWidth = data.score * 10;

                    var color;
                    if (!data.valid) {
                        color = bad;
                        scoreWidth = 0;
                    } else if (data.score < 5)
                        color = weak;
                    else if (data.score < 7)
                        color = medium;
                    else if (data.score < 9)
                        color = strong;
                    else
                        color = verystrong;

                    var sb = [];
                    sb.push("<strong>Password rating:</strong><br>");
                    sb.push("<div style='width: 100px; height: 10px; border: 1px solid #ccc; border-radius: 2px;'>");
                    sb.push("<div style='width: " + scoreWidth + "px; background-color: " + color + "; height: 10px; border-radius: 2px;'></div>");
                    sb.push("</div>");
                    sb.push("<small>[ " + data.score + " / 10 ] " + data.message + "</small>");
                    targetElement.html(sb.join(""));
                });
            }
        });
    };

}(jQuery));