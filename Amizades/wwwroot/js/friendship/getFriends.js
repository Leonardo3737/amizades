let friendsPage = 1;

function loadMoreFriends() {
    $.get(`/api/Friendship/friend?page=${friendsPage}`, function (html) {
        friendsPage++;
        const $container = $('#friends-container');
        $container.append(html);

        if (html.includes('id="hide-load-more-FRIEND"')) {
            $('#load-more-FRIEND').hide();
        }
    });
}