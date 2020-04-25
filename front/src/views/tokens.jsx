import React from 'react';

// reactstrap components
import { 
  Button,
  Table,
  Breadcrumb,
  BreadcrumbItem,
  Input,
  InputGroup,
  InputGroupAddon,
  InputGroupText,
  FormGroup,
  Label,
  Container,
  Pagination,
  PaginationItem,
  PaginationLink,
  Row
} from 'reactstrap';

// core components
import ExamplesNavbar from "components/Navbars/ExamplesNavbar.js";
import Footer from "components/Footer/Footer.js";

const Tokens = () => {
  return (
    <>
      <ExamplesNavbar />
      <div className="wrapper">
        <Container className="top-bar">
          <Breadcrumb>
            <BreadcrumbItem><a href="/">Home</a></BreadcrumbItem>
            <BreadcrumbItem active>Tokens</BreadcrumbItem>
          </Breadcrumb>
          <h1>Tokens de configuração</h1>
          <div className="table-filter">
            <form style={{display: 'flex', alignItems: 'baseline', justifyContent: 'space-between'}}>
              <FormGroup>
                <Label for="enviroment">Filtre por token</Label>
                <InputGroup>
                  <Input
                    type="text"
                    placeholder="Buscar por token"
                  />
                  <InputGroupAddon addonType="append">
                    <InputGroupText>
                      <i className="tim-icons icon-zoom-split" />
                    </InputGroupText>
                  </InputGroupAddon>
                </InputGroup>
              </FormGroup>
              <FormGroup>
                <Label for="enviroment">Selecione o ambiente</Label>
                <Input type="select" name="select" id="enviroment">
                  <option>Produção</option>
                  <option>Pré-Prod</option>
                  <option>Migração</option>
                  <option>Rockets</option>
                  <option>QA</option>
                </Input>
              </FormGroup>
            </form>
          </div>
        </Container>
        <div className="main">
          <Container>
            <Table responsive>
              <thead>
                <tr>
                  <th className="text-center">Token</th>
                  <th>Valor</th>
                  <th>Distribuições</th>
                  <th className="text-center">Ambiente</th>
                  <th className="text-right">Ultima atualização</th>
                  <th className="text-right">Usuário</th>
                  <th className="text-right">Ações</th>
                </tr>
              </thead>
              <tbody>
                {[1,2,3,4,5,6,7,8,9,10,11,12,13,14,15].map((line) => (
                  <tr>
                    <td className="text-center">configuration.settings.app[key]</td>
                    <td>12345</td>
                    <td>LegalOne - FirmBR</td>
                    <td className="text-center">Produção</td>
                    <td className="text-right">17/04/2020</td>
                    <td className="text-right">Vinícius Fontoura</td>
                    <td className="text-right">
                      <Button className="btn-icon btn-round" color="success" size="sm">
                        <i className="fa fa-edit"></i>
                      </Button>{` `}
                      <Button className="btn-icon btn-round" color="danger" size="sm">
                        <i className="fa fa-times" />
                      </Button>
                    </td>
                  </tr>
                ))}
              </tbody>
            </Table>
          </Container>
        </div>
        <Container>
          <Row className="justify-content-md-center">
            <Pagination>
              <PaginationItem disabled>
                <PaginationLink href="#">
                  Previous
                </PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationLink href="#">
                  1
                </PaginationLink>
              </PaginationItem>
              <PaginationItem active>
                <PaginationLink href="#">
                  2
                </PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationLink href="#">
                  3
                </PaginationLink>
              </PaginationItem>
              <PaginationItem>
                <PaginationLink href="#">
                  Next
                </PaginationLink>
              </PaginationItem>
            </Pagination>
          </Row>
        </Container>
      </div>
      <Footer />
    </>
  );
};

export default Tokens;