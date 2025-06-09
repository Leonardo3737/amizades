let suggestionPage = 1;
console.log('entroiaaaaaau')
function loadMorePublications() {
    console.log('entroiu')

    const button = $('#load-more-PUBLICATIONS')
    button.html(Spinner())
    button.prop('disabled', true)
    $.get(`/Publication/PublicationsPartial?page=${suggestionPage}`)
        .done(html => {
            suggestionPage++;
            const $container = $('#publications-container');
            $container.append(html);

            if (html.includes('id="hide-load-more-PUBLICATIONS"')) {
                $('#load-more-PUBLICATIONS').hide();
            }
        })
        .fail(() => {
            alert("Erro ao carregar mais publicações.");
        })
        .always(() => {
            button.html('Ver mais')
            button.prop('disabled', false)
        });
}