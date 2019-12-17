import authService from '../api-authorization/AuthorizeService';

export class Subscription {
    constructor(readonly url: string) {
      this.url = encodeURI(url);
    }
}

export interface Podcast {
    url: string,
    lastFetchTime: string
}

export async function subscribeToPodcast(subscription: Subscription) {
    const token = await authService.getAccessToken();
    await fetch('Api/1.0/Subscribe', {
        method: 'post',
        headers: !token ? {} : { 
        'Authorization': `Bearer ${token}`,
        'Content-type': 'application/json'
        },
        body: JSON.stringify(subscription)
    });
}

export async function getPodcasts() : Promise<Podcast[]> {
    const token = await authService.getAccessToken();
    const response = await fetch('Api/1.0/Podcasts', {
        headers: !token ? {} : { 'Authorization': `Bearer ${token}` } 
    });
    const data = await response.json();
    return data as Podcast[];
}