(function() {
    'use strict;'

    angular.module('app').controller('FindRoomController', ['Restangular', '$state', 'settings', '$timeout', '$q', '$modal', function(Restangular, $state, settings, $timeout, $q, $modal) {
        if(!settings.defaultRoom) {
            $state.go('settings');
            return;
        }
        var self = this;
        self.isLoading = true;
        self.rooms = [];
        self.search = {
            minSize: 1
        };
        self.locationChoices = [];
        Restangular.all('roomList').getList().then(function(roomLists) {
            return $q.all(roomLists.map(function(roomList) {
                return Restangular.one('roomList', roomList.Address).getList('rooms').then(function(rooms) {
                    if(rooms.filter(function(r) { return r.Address == settings.defaultRoom; }).length) {
                        // this is the right list - let's get our details
                        var allRooms = [];
                        return $q.all(rooms.map(function(room) {
                            return $q.all([
                                Restangular.one('room', room.Address).one('info').get({ securityKey: '' }),
                                Restangular.one('room', room.Address).one('status').get()
                            ]).then(function(calls){
                                allRooms.push(angular.merge({}, calls[0], calls[1]));
                            });
                        })).then(function() {
                            allRooms.sort(function(a, b) { return a.DisplayName < b.DisplayName ? -1 : 1; });
                            self.rooms = allRooms;
                        });
                    }
                    else {
                        // wrong list - do nothing
                    }
                });
            }));
        }).then(function() {
            self.isLoading = false;
        }).catch(function() {
            $state.go('settings');
        });
        
        self.showDetails = function showDetails(meeting) {
            $modal.open({
                templateUrl: 'meeting/meeting.html',
                controller: 'MeetingController',
                controllerAs: 'c',
                resolve: {
                    meeting: function() {
                        return meeting;
                    }
                }
            });
        };

        // we have a default room we're supposed to be managing - time out and go there after 60 seconds
        $timeout(function() {
            //$state.go('room');
        }, 60000);
    }]);
})();