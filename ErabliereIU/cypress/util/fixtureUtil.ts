export class FixtureUtil {
    static getRandomEmail() {
        const r1 = Math.floor(Math.random() * 1000);
        const r2 = Math.floor(Math.random() * 1000);
        return "random-email-" + r1 + "@domaine" + r2 + ".net";
    }
    static getFirstName(): string | undefined {
        const firstNames = [
            "John", "David", "Cindy", "Carole", "Fred", "Frank", "Jean"
        ];

        return firstNames[(Math.random() * 1000) % firstNames.length];
    }
}