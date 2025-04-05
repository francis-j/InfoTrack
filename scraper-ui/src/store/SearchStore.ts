import type SearchModel from "../models/SearchModel.ts";

export const loadSearchHistory = () => {
    const json = localStorage.getItem("searchHistory");
    if (json) {
        const history = JSON.parse(json);
        return history;
    }

    return [];
}

export const saveSearchHistory = (history: SearchModel[]) => {
    localStorage.setItem("searchHistory", JSON.stringify(history));
}