import type SearchModel from "../models/SearchModel.ts";

export const search = async (searchEngine: number, keywords: string, url: string): Promise<SearchModel> => {
    const res = await fetch(`${import.meta.env.VITE_API_URL}/api/search?searchEngine=${searchEngine}&keywords=${keywords}&url=${url}`);
    return res.ok
        ? await res.json()
        : { result: "" };
}