let suggestionPage = 1;

function loadMoreSugestions() {
    $.get(`/api/Friendship/suggestion?page=${suggestionPage}`, (html) => {
        suggestionPage++;
        const $container = $('#suggestions-container');
        $container.append(html);

        if (html.includes('id="hide-load-more-SUGESTION"')) {
            $('#load-more-SUGESTION').hide();
        }
    });
}