$(document).ready(function () {
    $('section').each(function (i, section) {
        var id = section.id;
        var heading = $('.section-header', section).text();

        $('<li><a href="#' + id + '"><i class="icon-chevron-right"></i> ' + heading + '</a>').appendTo("#page-toc");
    });

    $('#page-toc').affix();
});