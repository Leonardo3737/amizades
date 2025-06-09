function sendingInvite(userId) {
    $.post(`/api/friendship/invite/${userId}`, () => {
        const $btn = $(`#invite-btn-${userId}`);
        $btn.text("Enviado").prop("disabled", true);
    });
}

function acceptInvite(friendShipId) {
    $.ajax({
        url: `/api/friendship/accept/${friendShipId}`,
        method: "PATCH",
        success: () => {
            const $userCard = $(`#user-${friendShipId}-REQUEST`);
            const $clone = $userCard.clone();
            const $friendsContainer = $('#friends-container');
            const currentUserCount = changeUserCount(1);

            if (currentUserCount === 1) {
                $friendsContainer.html("");
            }

            const newId = `user-${friendShipId}-FRIEND`;
            $clone.attr("id", newId);

            const $button = $clone.find("button");
            console.log($button)
            if ($button.length) {
                $button.eq(1).text("Excluir").off("click").on("click", () => {
                    removeFriend(friendShipId);
                });
            }

            $friendsContainer.append($clone);
            $button.eq(0).remove()
            $userCard.remove();
        }
    });
}

function removeFriend(friendShipId, type) {
    $.ajax({
        url: `/api/friendship/remove/${friendShipId}`,
        method: "DELETE",
        success: () => {
            const $userCard = $(`#user-${friendShipId}-${type}`);
            if (type === "FRIEND") {
                const currentUserCount = changeUserCount(-1);
            }
            $userCard.remove();
        }
    });
}

function changeUserCount(valueChange) {
    const $friendCount = $('#friend-count');
    const current = Number($friendCount.text());

    if (!isNaN(current)) {
        $friendCount.text(current + valueChange);
    }

    return current + valueChange;
}