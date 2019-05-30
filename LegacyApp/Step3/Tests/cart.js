import { Selector } from 'testcafe';

fixture `Shopping Cart`
    .page `http://localhost:3254/`;

test('Clicking Add To Cart adds item to cart', async t => {
    const row = Selector('table').find('tr').nth(0);
    const name = await row.find('td').nth(0).textContent;
    const price = await row.find('td').nth(1).textContent;

    await t
        .click('#MenuRepeater_ctl00_AddToOrderButton')
        .expect(Selector('table').find('td').nth(0).textContent).eql(name)
        .expect(Selector('table').find('td').nth(2).textContent).eql('1')
        .expect(Selector('table').find('td').nth(1).textContent).eql(price);
});

test('Clicking Add To Cart twice increments quantity', async t => {
    const row = Selector('table').find('tr').nth(0);
    const name = await row.find('td').nth(0).textContent;
    const price = await row.find('td').nth(1).textContent;

    await t
        .click('#MenuRepeater_ctl00_AddToOrderButton')
        .navigateTo('/')
        .click('#MenuRepeater_ctl00_AddToOrderButton')
        .expect(Selector('table').find('td').nth(0).textContent).eql(name)
        .expect(Selector('table').find('td').nth(2).textContent).eql('2')
        .expect(Selector('table').find('td').nth(1).textContent).eql(price);
});