import React, { Component } from 'react';
import { Podcast, subscribedPodcasts } from '../api-podgrasp';

interface LibraryProps {}

interface LibraryState {
    podcasts: Podcast[],
    isLoading: boolean,
    error: string | null 
}

export class Library extends Component<LibraryProps, LibraryState> {
  
    constructor(props: LibraryProps) {
    
        super(props);
        
        this.state = {
            podcasts: [] as Podcast[],
            isLoading: false,
            error: null
        };
    
        this.fetchPodcasts = this.fetchPodcasts.bind(this);
        this.setPodcasts = this.setPodcasts.bind(this);
    }

    componentDidMount() {
        this.fetchPodcasts();
    }

    async fetchPodcasts() {
        try {
            this.setState({ isLoading: true });
            const podcasts = await subscribedPodcasts();
            this.setPodcasts(podcasts);
        }
        catch (error) {
            console.error(error);
        }
    }
    
    setPodcasts(result: Podcast[]) {

        this.setState({
            podcasts: result,
            isLoading: false
        });
    }

    static renderPodcastsTable(podcasts: Podcast[]) {
        return (
            <table>
                <thead>
                    <tr>
                        <td>URL</td>
                        <td>Last Fetch Time</td>
                    </tr>
                </thead>
                <tbody>
                    {podcasts.map(podcast =>
                        <tr>
                            <td>{podcast.url}</td>
                            <td>{podcast.lastFetchTime}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        return (
            <div>
                {Library.renderPodcastsTable(this.state.podcasts)}
            </div>
        );
    }

}
